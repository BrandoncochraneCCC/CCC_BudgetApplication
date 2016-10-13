using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BudgetFramework
{
    //---Levels---
    //ALL
    //DEBUG
    //INFO
    //WARN
    //ERROR
    //FATAL
    class Log4Net
    {

        public static readonly log4net.ILog log = Log4Net.GetLogger();

        //copy paste into start location
        //[assembly: log4net.Config.XmlConfigurator(Watch = true)]
        public static log4net.ILog GetLogger([CallerFilePath]string filename = "")
        {
            return log4net.LogManager.GetLogger(filename);
        }
    }
}
//---------------------------
//LOG4NET CONFIG SETTINGS //paste this into web.config
//---------------------------

//<!--log4net start-->
//  <!--Appenders start-->
//  <!-- ***************
//    Debug levels
//    **************
//    ALL
//    DEBUG
//    INFO
//    WARN
//    ERROR
//    FATAL
//    ************** -->
//  <log4net>
//    <!-- console appender, for debugging -->
//    <appender name = "ConsoleAppender" type="log4net.Appender.ConsoleAppender">
//      <filter type = "log4net.Filter.LevelRangeFilter" >
//        < levelMin value="DEBUG" />
//        <levelMax value = "DEBUG" />
//      </ filter >
//      < layout type="log4net.Layout.patternLayout">
//        <conversionPattern value = "Message - %message%newline                            Location - %location%                            newlineLine number - %line%newline                            Method - %method%newline                            Exception - %exception%newline                            %newline" />
//      </ layout >
//    </ appender >


//    < !--info file appender, for information logging-->
//    <appender name = "InfoFileAppender" type="log4net.Appender.RollingFileAppender">
//      <filter type = "log4net.Filter.LevelRangeFilter" >
//        < levelMin value="INFO" />
//        <levelMax value = "INFO" />
//      </ filter >
//      < file value="C:\Logs\InfoLog.txt" />
//      <appendtoFile value = "true" />
//      < lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
//      <maximumFileSize value = "5MB" />
//      < staticLogFileName value="True" />
//      <layout type = "log4net.Layout.patternLayout" >
//        < conversionPattern value="---------------------------------------------%newline                            Level - %level%newline                            Message - %message%newline                            UTC Date - %utcdate{ABSOLUTE}%newline                            Location - %location%newline                            Line number - %line%newline                            Method %method%newline                            Exception - %exception%newline                            %newline" />
//      </layout>
//    </appender>
    
//    <!-- rolling file appender, for warnings, errors, and fatal messages -->
//    <appender name = "RollingFileAppender" type="log4net.Appender.RollingFileAppender">
//      <filter type = "log4net.Filter.LevelRangeFilter" >
//        < levelMin value="WARN" />
//        <levelMax value = "FATAL" />
//      </ filter >
//      < file value="C:\Logs\RollingFileLog.txt" />
//      <appendtoFile value = "true" />
//      < rollingStyle value="Size" />
//      <maximumFileSize value = "10MB" />
//      < maxSizeRollBackups value="5" />
//      <staticLogFileName value = "True" />
//      < layout type="log4net.Layout.patternLayout">
//        <conversionPattern value = "---------------------------------------------%newline                            Level - %level%newline                            Message - %message%newline                            UTC Date - %utcdate{ABSOLUTE}%newline                            Date - %date{ABSOLUTE}%newline                            Timestamp - %timestamp%newline                            Identity - %identity%newline                            Username - %username%newline                            Location - %location%newline                            Line number - %line%newline                            Method %method%newline                            Exception - %exception%newline                            %newline" />
//      </ layout >
//    </ appender >
//    < !--ado net appender -->
//    <!-- NOT FUNCTIONAL -->
//    <!-- used to add log item into database -->
//    <!--<appender name = "AdoNetAppender" type="log4net.Appender.AdoNetAppender">
//      <bufferSize value = "1" />
//      < connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
//      <connectionString value = "data source=(localdb)\ProjectsV13;initial catalog=BudgetData;integrated security=True;" />
//      < commandText value="[dbo].[procLog_Insert]" />
//      <commandType value = "StoredProcedure" />
//      < parameter >
//        < parameterName value="@log_date" />
//        <dbType value = "DateTime" />
//        < layout type="log4net.Layout.RawTimeStampLayout" />
//      </parameter>
//      <parameter>
//        <parameterName value = "@log_thread" />
//        < dbType value="String" />
//        <size value = "50" />
//        < layout type="log4net.Layout.PatternLayout">
//          <conversionPattern value = "%thread" />
//        </ layout >
//      </ parameter >
//      < parameter >
//        < parameterName value="@log_level" />
//        <dbType value = "String" />
//        < size value="50" />
//        <layout type = "log4net.Layout.PatternLayout" >
//          < conversionPattern value="%level" />
//        </layout>
//      </parameter>
//      <parameter>
//        <parameterName value = "@log_source" />
//        < dbType value="String" />
//        <size value = "300" />
//        < layout type="log4net.Layout.PatternLayout">
//          <conversionPattern value = "%logger" />
//        </ layout >
//      </ parameter >
//      < parameter >
//        < parameterName value="@log_message" />
//        <dbType value = "String" />
//        < size value="4000" />
//        <layout type = "log4net.Layout.PatternLayout" >
//          < conversionPattern value="%message" />
//        </layout>
//      </parameter>
//      <parameter>
//        <parameterName value = "@exception" />
//        < dbType value="String" />
//        <size value = "4000" />
//        < layout type="log4net.Layout.ExceptionLayout" />
//      </parameter>
//    </appender>-->
//    <!--Appenders end-->
//    <!--root start-->
//    <root>
//      <level value = "DEBUG" />
//      < appender -ref ref="ConsoleAppender" />
//      <appender-ref ref="InfoFileAppender" />
//      <appender-ref ref="RollingFileAppender" />
//      <!-- <appender-ref ref="AdoNetAppender" /> -->
//    </root>
//    <!--root end-->
//    <appender name = "aiAppender" type="Microsoft.ApplicationInsights.Log4NetAppender.ApplicationInsightsAppender, Microsoft.ApplicationInsights.Log4NetAppender">
//      <layout type = "log4net.Layout.PatternLayout" >
//        < conversionPattern value="%message%newline" />
//      </layout>
//    </appender>
//  </log4net>
//  <!--log4net end-->  