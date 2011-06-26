root_path = File.dirname(File.dirname(__FILE__)) + "/"

require 'erb'
require 'yaml'
require File.join(root_path, 'build/nuget.rb')
require File.join(root_path, 'build/albacore/albacore.rb')

#ENV['nightly']    = 'false'
#ENV['nuget_api_key'] = ''
config = nil

task :configure do

	b = binding
	# read VERSION from file
	base_version = nil
	File.open("#{root_path}VERSION",'r') do |f|
	   base_version = f.gets.chomp
	end

	build_number = ENV['BUILD_NUMBER'] || "0"
	version_full = "#{base_version}.#{build_number}"

	# load config.yaml
	config = YAML.load(ERB.new(File.read(File.join(root_path, 'build/config.yml'))).result(b))

	# begin
    #     config["vcs"]["rev_id"] = `hg id -i`.chomp.chop # remove the +
    #     config["vcs"]["name"] = 'hg'
    #     config["vcs"]["short_rev_id"] = config[:vcs][:rev_id]
    # rescue
    # end
	
	if(config["vcs"]["rev_id"] == "") then
		# if mercurial fails try git
		begin
			config["vcs"]["rev_id"]    = `git log -1 --pretty=format:%H`.chomp
			config["vcs"]["name"] = 'git'
			config["vcs"]["short_rev_id"] = config["vcs"]["rev_id"][0..7]
		rescue
		end
	end
	
	if(config["version"]["is_nightly"])
        config["version"]["long"] = "#{config["version"]["full"]}-nightly-#{config["vcs"]["short_rev_id"]}"
    else
		config["version"]["long"] = "#{config["version"]["full"]}-#{config["vcs"]["short_rev_id"]}"
    end
	
	puts
    puts  "     Project Name: #{config["project"]["name"]}"
    puts  "Safe Project Name: #{config["project"]["safe_name"]}"
	puts  "          Version: #{config["version"]["full"]} (#{config["version"]["long"]})"
	puts  "     Base Version: #{config["version"]["base"]}"
	print "  CI Build Number: #{config["version"]["build_number"]}"
	print " (not running under CI mode)" if config["version"]["build_number"] == "0"
	puts
    puts  "        Root Path: #{config["path"]["root"]}"
    puts
	puts  "              VCS: #{config["vcs"]["name"]}"
    print "      Revision ID: #{config["vcs"]["rev_id"]}"
    print "  (#{config["vcs"]["short_rev_id"]})" if config["vcs"]["name"] == 'git'
    puts    
    puts   
	
end

Rake::Task["configure"].invoke

directory "#{config['path']['working']}"
directory "#{config['path']['working']}NuGet/"
directory "#{config['path']['working']}SymbolSource/"
directory "#{config['path']['dist']}"
directory "#{config['path']['dist']}NuGet/"
directory "#{config['path']['dist']}SymbolSource/"

namespace :build do
	
	desc "Build .NET 4 binaries"
	msbuild :net40 => ['clean:net40','assemblyinfo:fluenthttp'] do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net40']
	   msb.targets :Build
	end
	
	desc "Build .NET 3.5 binaries"
	msbuild :net35 => ['clean:net35','assemblyinfo:fluenthttp'] do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net35']
	   msb.targets :Build
	   msb.use :net35
	end

	desc "Build .NET 2.0 binaries"
	msbuild :net20 => ['clean:net20','assemblyinfo:fluenthttp'] do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net20']
	   msb.targets :Build
	   msb.use :net35
	end
	
	desc "Build Silverlight 4 binaries"
	msbuild :sl4 => ['clean:sl4','assemblyinfo:fluenthttp'] do |msb|	   
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['sl4']
	   msb.targets :Build
	end
	
	desc "Build Windows Phone 7 binaries"
	msbuild :wp7 => ['clean:wp7','assemblyinfo:fluenthttp'] do |msb|
	  msb.properties :configuration => config['version']['configuration']
	  msb.solution = config['path']['sln']['wp7']
	  msb.targets :Build
	end
	
	msbuild :docs => ['build:net40'] do |msb|
		msb.properties :configuration => config['version']['configuration']
		msb.properties :DocumentationSourcePath => "#{ config['path']['output']}Release/Net40/"
		msb.solution = config['path']['sln']['shfb']
		msb.targets [:Clean,:Rebuild]
		msb.properties
	end
	
	multitask :all => ['build:net20','build:net35','build:net40','build:sl4','build:wp7']
	
end

namespace :zip do
	
	zip :libs => ['build:all',"#{config['path']['dist']}"] do |zip|
		zip.directories_to_zip "#{config['path']['output']}Release/"
		zip.output_file = "#{config['project']['safe_name']}-#{config['version']['long']}.bin.zip"
		zip.output_path = "#{config['path']['dist']}"
		zip.additional_files = [ 
									"#{config['path']['root']}LICENSE",
									"#{config['path']['root']}VERSION"
							   ]
	end
	
	desc "Create zip archive of the source files"
	task :source => ["#{config['path']['dist']}"] do
		src_archive_name = "#{config['path']['dist']}#{config['project']['safe_name']}-#{config['version']['long']}.src.zip"
		if (config["vcs"]["name"] == 'git') then
			sh "git archive HEAD --format=zip > \"#{src_archive_name}\""
		elsif (config["vcs"]["name"] == 'hg') then
			sh "hg archive -tzip \"#{src_archive_name}\" -p \"#{PROJECT_NAME_SAFE}\""
		end
	end
	
	multitask :all => ['zip:libs','zip:source']
	
end

desc "Build All"
task :build => ['build:all']

namespace :clean do

	msbuild :net40 do |msb|
	   msb.properties :configuration => config["version"]["configuration"]
	   msb.solution = config["path"]["sln"]["net40"]
	   msb.targets :Clean
	end
	
	msbuild :net35 do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net35']
	   msb.targets :Clean
	   msb.use :net35
	end

	msbuild :net20 do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net20']
	   msb.targets :Clean
	   msb.use :net35
	end
	
	msbuild :sl4 do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['sl4']
	   msb.targets :Clean    
	end
	
	msbuild :wp7 do |msb|
		msb.properties :configuration => config['version']['configuration']
		msb.solution = config['path']['sln']['wp7']
		msb.use :net40
		msb.targets :Clean
	end
	
	multitask :libs => ['clean:net20','clean:net35','clean:net40','clean:sl4','clean:wp7']
	
	multitask :all => ['clean:libs'] do
		FileUtils.rm_rf config["path"]["output"]
		FileUtils.rm_rf config["path"]["working"]
		FileUtils.rm_rf config["path"]["dist"]
	end
	
end

desc "Clean All"
task :clean => ['clean:all']

desc "Build All Libraries (default)"
multitask :libs => ['clean:libs','build']

task :default => ['libs']

namespace :assemblyinfo do

	assemblyinfo :fluenthttp do |asm|
		asm.output_file = "#{config["path"]["src"]}FluentHttp/Properties/AssemblyInfo.cs"
		asm.version = config["version"]["full"]
		asm.title = "FluentHttp"
		asm.description = "Fluent http wrapper for .NET"
		asm.product_name = "FluentHttp"
		asm.company_name = "Prabir Shrestha"
		asm.copyright = "Apache License v2.0"
		asm.com_visible = false
	end
	
	multitask :all => ["assemblyinfo:fluenthttp"]
	
end

task :assemblyinfo => ['assemblyinfo:all']

namespace :nuget do

	task :nuspec => ["#{config['path']['working']}","#{config['path']['working']}NuGet/","#{config['path']['working']}SymbolSource/"] do

		# copy authenticator files
		cp "#{config['path']['src']}FluentHttp.Tests/FluentAuthenticators/HttpBasicAuthenticator.cs", "#{config['path']['working']}NuGet/HttpBasicAuthenticator.cs.pp"
		cp "#{config['path']['src']}FluentHttp.Tests/FluentAuthenticators/OAuth2Authenticator.cs", "#{config['path']['working']}NuGet/OAuth2Authenticator.cs.pp"
		
		FileList.new("#{config["path"]["working"]}NuGet/*.cs.pp").each do |path|
			s = nil
			open(path,'r') { |f| s = f.read }
			open(path,'w') { |f| f.puts s.gsub(/^namespace FluentHttp.Authenticators$/, "namespace $rootnamespace$.FluentHttp.Authenticators") }
		end

		FileList.new("#{config["path"]["build"]}*/*/*.nuspec").each do |path|
			outfile = "#{config['path']['working']}" +
                 (path.start_with?("#{config['path']['build']}NuGet") ? "NuGet/" : "SymbolSource/") +
				 File.basename(path)
			File.open("#{outfile}","w") do |f|
				b = binding
				version_full = config['version']['full']
				f.write(ERB.new(File.read(path)).result(b))
				f.close()
				puts "Created #{outfile}"
			end
		end
	
	end
	
	multitask :pack => ['nuget:nuspec','libs',"#{config['path']['dist']}","#{config['path']['dist']}NuGet/","#{config['path']['dist']}SymbolSource/"] do
	
		FileList.new("#{config["path"]["working"]}*/*.nuspec").each do |path|
			nugetpack 'nuget:pack' do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.nuspec  = path            
				nuget.output = "#{config['path']['dist']}" +
				(path.start_with?("#{config['path']['working']}NuGet") ? "NuGet/" : "SymbolSource/")
			end
		end
	
	end

	desc "Push .nupkg to symbol source & publish"
	task :push_source do		
		FileList.new("#{config["path"]["build"]}SymbolSource/*").each do |path|
			nugetpush ['nuget:push_source'] do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.package = "#{config['path']['dist']}SymbolSource/#{File.basename(path)}.#{config['version']['full']}.nupkg"
				nuget.apikey = ENV['nuget_api_key']
				nuget.source = "http://nuget.gw.symbolsource.org/Public/NuGet"
			end
		end
	end
	
	desc "Push .nupkg to nuget.org but don't publish"
	task :push do
		FileList.new("#{config["path"]["build"]}NuGet/*").each do |path|
			nugetpush ['nuget:push'] do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.package = "#{config['path']['dist']}NuGet/#{File.basename(path)}.#{config['version']['full']}.nupkg"
				nuget.apikey = ENV['nuget_api_key']
				nuget.create_only = true
			end
		end
	end
	
	desc "Publish .nupkg to nuget.org live feed"
	task :publish do
		FileList.new("#{config["path"]["build"]}NuGet/*").each do |path|
			nugetpublish ['nuget:publish'] do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.id = "#{File.basename(path)}"
				nuget.version = "#{config['version']['full']}"
				nuget.apikey = ENV['nuget_api_key']
			end
		end
	end
	
end

desc "Build NuGet packages"
task :nuget => ['nuget:pack']

desc "Create distribution packages"
task :dist => ['zip:all','nuget','build:docs']