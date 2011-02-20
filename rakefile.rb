require File.join(File.dirname(__FILE__), 'libs/albacore/albacore.rb')

task :default => [:libs]

PROJECT_NAME      = "Fluent Http"
PROJECT_NAME_SAFE = "FluentHttp"
LOG               = false
ENABLE_HG_CHECK   = false
#ENV['NIGHTLY']   = 'false'

build_config = nil

task :configure do
    # do configuration stuffs here
        
    root_path    = "#{File.dirname(__FILE__)}/"
    base_version = 0
   
    File.open("#{root_path}VERSION",'r') do |f|
        base_version = f.gets.chomp
    end
    
    build_config = {
        :log        => false,
        :paths      => {
            :root   => root_path,
            :src    => "#{root_path}src/",
            :output => "#{root_path}bin/",
            :build  => "#{root_path}build/",
            :dist   => "#{root_path}dist/"
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
        }        
    }
    
    build_config[:sln][:wp7]   = "#{build_config[:paths][:src]}FluentHttp-WP7.sln"
    build_config[:sln][:sl4]   = "#{build_config[:paths][:src]}FluentHttp-SL4.sln"
    build_config[:sln][:net40] = "#{build_config[:paths][:src]}FluentHttp-Net40.sln"
    build_config[:sln][:net35] = "#{build_config[:paths][:src]}FluenttHttp-Net35.sln"    
    
    #Albacore configure do |config|
    #    config log_level = :verbose if build_config[:log]
    #end
    
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
    # temporary hack for bug caused by code contracts
    FileUtils.rm_rf "#{build_config[:paths][:src]}FluentHttp/obj/"
    
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
    # temporary hack for bug caused by code contracts
    FileUtils.rm_rf "#{build_config[:paths][:src]}FluentHttp/obj/"
    
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35]
    msb.targets :Build
    #msb.use :net35
    # compile .net 3.5 libraries using msbuild 4.0 in order to generate the code contract libraries.
    # seems like if we use .net 3.5, it does not generate the code contracts.
end

msbuild :clean_net35 do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35]
    msb.targets :Clean
    #msb.use :net35
end

desc "Build Silverlight 4 binaries"
msbuild :sl4 => [:clean_sl4, :assemblyinfo_fluenthttp] do |msb|
    # temporary hack for bug caused by code contracts
    FileUtils.rm_rf "#{build_config[:paths][:src]}FluentHttp/obj/"
   
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
    # temporary hack for bug caused by code contracts
    FileUtils.rm_rf "#{build_config[:paths][:src]}FluentHttp/obj/"
    
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