require File.join(File.dirname(__FILE__), 'libs/albacore/albacore.rb')
require File.join(File.dirname(__FILE__), 'libs/albacore/dacopier.rb')

require 'find'
require 'fileutils'

task :default => [:libs]

PROJECT_NAME      = "Fluent Http"
PROJECT_NAME_SAFE = "FluentHttp"
LOG               = false
ENABLE_HG_CHECK   = false
#ENV['NIGHTLY']   = 'false'
#ENV['NUGET_FLUENTHTTP_API_KEY'] = ''

build_config = nil
nuspec_config = nil

task :configure do
    # do configuration stuffs here

	Albacore.configure do |config|
		config.log_level = :verbose if LOG
	end
        
    root_path    = "#{File.dirname(__FILE__)}/"
    base_version = 0
   
    File.open("#{root_path}VERSION",'r') do |f|
        base_version = f.gets.chomp
    end
    
    build_config = {
        :log			=> false,
        :paths			=> {
            :root		=> root_path,
            :src		=> "#{root_path}src/",
            :output		=> "#{root_path}bin/",
            :build		=> "#{root_path}build/",
            :dist		=> "#{root_path}dist/",
			:tools		=> "#{root_path}libs/",
			:working	=> "#{root_path}working/",
			:packages	=> '',
            :nuget		=> '',
            :xunit		=> {
                :x86_console_path => ''
            }
        },
        :version    => {
            :base   => "#{base_version}",
            :full   => "#{base_version}",
            :long   => "#{base_version}"
        },
        :vcs        => {
            :name         => '',
            :rev_id       => '',
            :short_rev_id => '',
        },
        :ci         => {
            :build_number_param_name => 'BUILD_NUMBER',
            :is_nightly              => true,
            :build_number            => 0
        },
        :configuration => :Release,
        :sln        => {
            :wp7    => '',
            :sl4    => '',
            :net40  => '',
            :net35  => '',
			:shfb   => '', # sandcastle help file builder doc project
        },
		:nuspec => {
            :authors => "Prabir Shrestha",
        }
    }

	nuspec_config = {
		"FluentHttp" => {
            :title => "Fluent Http",
			:description => ".NET rest client helper for http web requests."			
        }
	}
    
	build_config[:paths][:packages]  = "#{build_config[:paths][:src]}packages/"
	build_config[:paths][:nuget]  = "#{build_config[:paths][:packages]}NuGet.CommandLine.1.3.20425.372/Tools/NuGet.exe"

	build_config[:paths][:xunit][:x86_console_path]  = "#{build_config[:paths][:tools]}xunit/xunit.console.clr4.x86.exe"

    build_config[:sln][:wp7]   = "#{build_config[:paths][:src]}FluentHttp-WP7.sln"
    build_config[:sln][:sl4]   = "#{build_config[:paths][:src]}FluentHttp-SL4.sln"
    build_config[:sln][:net40] = "#{build_config[:paths][:src]}FluentHttp-Net40.sln"
    build_config[:sln][:net35] = "#{build_config[:paths][:src]}FluenttHttp-Net35.sln"    
    build_config[:sln][:shfb]  = "#{build_config[:paths][:src]}docs.shfbproj"

    if (ENABLE_HG_CHECK==true) then
        begin
            build_config[:vcs][:rev_id] = `hg id -i`.chomp.chop # remove the +
            build_config[:vcs][:name] = 'hg'
            build_config[:vcs][:short_rev_id] = build_config[:vcs][:rev_id]
        rescue
        end
    end
    
    if(build_config[:vcs][:rev_id].length==0) then
        # if mercurial fails try git
        begin
            build_config[:vcs][:rev_id]    = `git log -1 --pretty=format:%H`.chomp
            build_config[:vcs][:name] = 'git'
            build_config[:vcs][:short_rev_id] = build_config[:vcs][:rev_id][0..7]
        rescue
        end
    end
   
    build_config[:ci][:is_nightly]   = ENV['NIGHTLY'].nil? ? true : ENV['NIGHTLY'].match(/(true|1)$/i) != nil
    build_config[:ci][:build_number] = ENV[build_config[:ci][:build_number_param_name]] || 0
    
    build_config[:version][:full] = "#{build_config[:version][:base]}.#{build_config[:ci][:build_number]}"
   
    if(build_config[:ci][:is_nightly])
        build_config[:version][:long] = "#{build_config[:version][:full]}-nightly-#{build_config[:vcs][:short_rev_id]}"
    else
        build_config[:version][:long] = "#{build_config[:version][:full]}-#{build_config[:vcs][:short_rev_id]}"        
    end
   
    puts build_config if build_config[:log]
    puts
    puts "     Project Name: #{PROJECT_NAME}"
    puts "Safe Project Name: #{PROJECT_NAME_SAFE}"
    puts "          Version: #{build_config[:version][:full]} (#{build_config[:version][:long]})"
    puts "     Base Version: #{build_config[:version][:base]}"
    print "  CI Build Number: #{build_config[:ci][:build_number]}"
    print " (not running under CI mode)" if build_config[:ci][:build_number] == 0
    puts
    puts "        Root Path: #{build_config[:paths][:root]}"
    puts
    puts "              VCS: #{build_config[:vcs][:name]}"
    print "      Revision ID: #{build_config[:vcs][:rev_id]}"
    print "  (#{build_config[:vcs][:short_rev_id]})" if build_config[:vcs][:name] == 'git'
    puts    
    puts
    
end

Rake::Task["configure"].invoke

desc "Build .NET 4 binaries"
msbuild :net40 => [:clean_net40, :assemblyinfo_fluenthttp] do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net40]
    msb.targets :Build
end

msbuild :clean_net40 do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net40]
    msb.targets :Clean
end

desc "Build .NET 3.5 binaries"
msbuild :net35 => [:clean_net35, :assemblyinfo_fluenthttp] do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35]
    msb.targets :Build
    msb.use :net35
end

msbuild :clean_net35 do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35]
    msb.targets :Clean
    msb.use :net35
end

desc "Build Silverlight 4 binaries"
msbuild :sl4 => [:clean_sl4, :assemblyinfo_fluenthttp] do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:sl4]
    msb.targets :Build
end

msbuild :clean_sl4 do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:sl4]
    msb.targets :Clean    
end

desc "Build Windows Phone 7 binaries"
msbuild :wp7 => [:clean_wp7, :assemblyinfo_fluenthttp] do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:wp7]
    msb.targets :Build
end

msbuild :clean_wp7 do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:wp7]
    msb.targets :Clean
end

desc "Build All Libraries (default)"
task :libs => [:net35, :net40, :sl4,:wp7]

desc "Clean All"
task :clean => [:clean_net35, :clean_net40, :clean_sl4, :clean_wp7] do
   FileUtils.rm_rf build_config[:paths][:output]
   FileUtils.rm_rf build_config[:paths][:dist] 
   FileUtils.rm_rf build_config[:paths][:working]          
end

assemblyinfo :assemblyinfo_fluenthttp do |asm|
    asm.output_file = "#{build_config[:paths][:src]}FluentHttp/Properties/AssemblyInfo.cs"
    asm.version = build_config[:version][:full]
    asm.title = "FluentHttp"
    asm.description = "Fluent http wrapper for .NET"
    asm.product_name = "FluentHttp"
    asm.company_name = "Prabir Shrestha"
    asm.copyright = "Apache License v2.0"
    asm.com_visible = false
end

msbuild :docs => [:net40] do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Release/net40/" if build_config[:configuration] = :Release
   msb.solution = build_config[:sln][:shfb]
   msb.targets [:Clean,:Rebuild]
   msb.properties
end

msbuild :clean_docs do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Release/net40/" if build_config[:configuration] = :Release
   msb.solution = build_config[:sln][:shfb]
   msb.targets [:Clean]
   msb.properties
end

directory "#{build_config[:paths][:working]}"
directory "#{build_config[:paths][:working]}NuGet/"
directory "#{build_config[:paths][:dist]}"
directory "#{build_config[:paths][:dist]}NuGet"

task :dist_prepare do
	rm_rf "#{build_config[:paths][:dist]}"
    mkdir "#{build_config[:paths][:dist]}"

	rm_rf "#{build_config[:paths][:working]}"
	mkdir "#{build_config[:paths][:working]}"
end

desc "Create zip archive of the source files"
task :dist_source => [:dist_prepare] do
   src_archive_name = "#{build_config[:paths][:dist]}#{PROJECT_NAME_SAFE}-#{build_config[:version][:long]}.src.zip"
   if (build_config[:vcs][:name] == 'git') then
    sh "git archive HEAD --format=zip > \"#{src_archive_name}\""
   elsif (build_config[:vcs][:name] == 'hg') then
    sh "hg archive -tzip \"#{src_archive_name}\" -p \"#{PROJECT_NAME_SAFE}\""
   end
end

desc "Create distribution packages for libraries"
task :dist_libs => [:dist_prepare, :nuget] do
	 # copy nuget outputs
    mkdir "#{build_config[:paths][:dist]}NuGet/"
    mkdir "#{build_config[:paths][:dist]}SymbolSource/"
    cp Dir["#{build_config[:paths][:working]}NuGet/*.nupkg"], "#{build_config[:paths][:dist]}NuGet/"
    cp Dir["#{build_config[:paths][:working]}SymbolSource/*.nupkg"], "#{build_config[:paths][:dist]}SymbolSource/"

	# copy binary .dll files
    mkdir "#{build_config[:paths][:working]}Bin/"
    FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/FluentHttp/lib/.", "#{build_config[:paths][:working]}Bin/FluentHttp"

	zip :dist_libs do |zip|
        zip.directories_to_zip "#{build_config[:paths][:working]}Bin/"
        zip.output_file = "#{PROJECT_NAME_SAFE}-#{build_config[:version][:long]}.bin.zip"
        zip.output_path = "#{build_config[:paths][:dist]}"
        zip.additional_files = ["#{build_config[:paths][:root]}LICENSE"]
    end
end

task :nuspec => ["#{build_config[:paths][:working]}", :libs] do
	rm_rf "#{build_config[:paths][:working]}NuGet/"
    mkdir "#{build_config[:paths][:working]}NuGet/"

	Dir.entries(base_dir = "#{build_config[:paths][:build]}NuGet/").each do |name|
		path = "#{base_dir}#{name}/"
        dest_path = "#{build_config[:paths][:working]}NuGet/#{name}/"
        if name == '.' or name == '..' then
            next
        end
        FileUtils.cp_r path, dest_path
		FileUtils.rm "#{dest_path}PLACEHOLDER" if File.exist?("#{dest_path}PLACEHOLDER")

		nuspec do |nuspec|
			config = nuspec_config[name]
			nuspec.id = name
			nuspec.title = config[:title]
            nuspec.version = "#{build_config[:version][:full]}"
            nuspec.authors = "#{build_config[:nuspec][:authors]}"
            nuspec.description = config[:description]
            nuspec.language = "en-US"
            nuspec.licenseUrl = "https://github.com/prabirshrestha/FluentHttp/blob/master/LICENSE"
			nuspec.requireLicenseAcceptance = "false"
			nuspec.projectUrl = "https://github.com/prabirshrestha/FluentHttp"
			nuspec.tags = "web,REST,HTTP,API,services,wrapper"
			nuspec.output_file = "#{dest_path}/#{name}.nuspec"

			if !config[:dependencies].nil? then
                config[:dependencies].each do |d|
                    nuspec.dependency d[:id], d[:version]
                end
            end
		end
	end

	# copy libs for FluentHttp.dll
    DaCopier.new([]).copy "#{build_config[:paths][:output]}Release", "#{build_config[:paths][:working]}NuGet/FluentHttp/lib/"    

	# duplicate to SymbolSource folder
	rm_rf "#{build_config[:paths][:working]}SymbolSource/"
    mkdir "#{build_config[:paths][:working]}SymbolSource/" 
	FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/FluentHttp", "#{build_config[:paths][:working]}SymbolSource/FluentHttp"

	# remove pdb files from original NuGetFolder as it is present in SymbolSource folder instead
    FileUtils.rm Dir.glob("#{build_config[:paths][:working]}NuGet/*/**/*.pdb")

	# prepare to copy src to SymbolSource folder
    mkdir "#{build_config[:paths][:working]}SymbolSource/FluentHttp/src"

	# copy the source codes
    DaCopier.new(["^obj$","packages.config",".cd",".user",".suo"]).copy "#{build_config[:paths][:src]}FluentHttp/", "#{build_config[:paths][:working]}SymbolSource/FluentHttp/src/"
end

task :nuget => [:nuspec] do
	# copy nuspec files from NuGet to SymbolSource
	cp "#{build_config[:paths][:working]}NuGet/FluentHttp/FluentHttp.nuspec", "#{build_config[:paths][:working]}SymbolSource/FluentHttp/"

	Dir["#{build_config[:paths][:working]}*/*/*.nuspec"].each do |name|
        nugetpack :nuget do |nuget|
            nuget.command = "#{build_config[:paths][:nuget]}"
            nuget.nuspec  = name            
            nuget.output = "#{build_config[:paths][:working]}" +
                 (name.start_with?("#{build_config[:paths][:working]}NuGet") ? "NuGet/" : "SymbolSource")
        end
    end
end
