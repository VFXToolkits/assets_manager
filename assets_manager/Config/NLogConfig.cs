using NLog;
using System;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace assets_manager.Config;

public class NLogConfig
{
    public enum LogLevel
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug,
        Trace,
    }

    static string NLOG_CONFIG_FILE_NAME = $"{AppPathConst.BaseDirectory}NLog.config";
    const string TARGET_MIN_LEVEL_ATTRIBUTE = "minlevel";
    const string LOGGER_FILE_NAME_ATTRIBUTE = "fileName";

    XmlDocument doc = new XmlDocument();
    XmlElement? logFileNameElement;
    XmlElement? logLevelElement;

    /// <summary>
    /// Load the NLog config xml file content
    /// </summary>
    public static NLogConfig LoadXML()
    {
        NLogConfig config = new NLogConfig();
        config.doc.Load(NLOG_CONFIG_FILE_NAME);
        config.logLevelElement = (XmlElement)SelectSingleNode(config.doc, "//nlog:logger[@name='*']");
        config.logFileNameElement = (XmlElement)SelectSingleNode(config.doc, "//nlog:target[@name='defaultFile']");
        return config;
    }

    /// <summary>
    /// Save the content to NLog config xml file
    /// </summary>
    public static void SaveXML(NLogConfig nLogConfig)
    {
        nLogConfig.doc.Save(NLOG_CONFIG_FILE_NAME);
    }


    /// <summary>
    /// Get the current minLogLevel from xml file
    /// </summary>
    /// <returns></returns>
    public LogLevel GetLogLevel()
    {
        LogLevel level = LogLevel.Warn;
        if (logLevelElement == null)
            return level;
        string levelStr = logLevelElement.GetAttribute(TARGET_MIN_LEVEL_ATTRIBUTE);
        Enum.TryParse(levelStr, out level);
        return level;
    }

    /// <summary>
    /// Get the target fileName from xml file
    /// </summary>
    /// <returns></returns>
    public string GetLogFileName()
    {
        return logFileNameElement.GetAttribute(LOGGER_FILE_NAME_ATTRIBUTE);
    }

    /// <summary>
    /// Set the minLogLevel to xml file
    /// </summary>
    /// <param name="logLevel"></param>
    public void SetLogLevel(LogLevel logLevel)
    {
        logLevelElement.SetAttribute(TARGET_MIN_LEVEL_ATTRIBUTE, logLevel.ToString("G"));
    }

    /// <summary>
    /// Set the target fileName to xml file
    /// </summary>
    /// <param name="fileName"></param>
    public void SetLogFileName(string fileName)
    {
        logFileNameElement.SetAttribute(LOGGER_FILE_NAME_ATTRIBUTE, fileName);
    }

    /// <summary>
    /// Select a single XML node/elemant
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    private static XmlNode SelectSingleNode(XmlDocument doc, string xpath)
    {
        XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
        manager.AddNamespace("nlog", "http://www.nlog-project.org/schemas/NLog.xsd");
        //return doc.SelectSingleNode("//nlog:logger[(@shadowsocks='managed') and (@name='*')]", manager);
        return doc.SelectSingleNode(xpath, manager);
    }

    /// <summary>
    /// Extract the pre-defined NLog configuration file is does not exist. Then reload the Nlog configuration.
    /// </summary>
    public static void TouchAndApplyNLogConfig()
    {
        try
        {
            if (File.Exists(NLOG_CONFIG_FILE_NAME)) { 
                LoadConfiguration();
                return; // NLog.config exists, and has already been loaded
            }

            File.WriteAllText(NLOG_CONFIG_FILE_NAME, NLog_config_defaultFile);
        }
        catch (Exception ex)
        {
            NLog.Common.InternalLogger.Error(ex, "[assets manager] Failed to setup default NLog.config: {0}", NLOG_CONFIG_FILE_NAME);
            return;
        }

        LoadConfiguration();    // Load the new config-file
    }

    /// <summary>
    /// NLog reload the config file and apply to current LogManager
    /// </summary>
    public static void LoadConfiguration()
    {
        LogManager.LoadConfiguration(NLOG_CONFIG_FILE_NAME);
        LogManager.Configuration.Variables["sys_temp_dir"] = AppPathConst.AppTempPath;
    }

    const string NLog_config_defaultFile = @"<?xml version=""1.0"" encoding=""utf-8""?>

<nlog xmlns=""http://www.nlog-project.org/schemas/NLog.xsd""
      xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
      throwConfigExceptions=""true""
      throwExceptions=""true""
      autoReload=""true"">

  <targets>
    <target name=""defaultFile"" xsi:type=""File""
            fileName=""${var:sys_temp_dir}/assets_manager_logs/${date:format=yyyy-MM-dd}_log.log"" encoding=""utf-8""
            layout=""${date}|${callsite}|${uppercase:${level}}|${replace:inner=${message}:searchFor=\\r\\n|\\s:replaceWith= :regex=true}|${replace:inner=${exception:format=ToString}:searchFor=\\r\\n|\\s:replaceWith= :regex=true}"" />
  </targets>

  <rules>
    <logger name=""*"" minlevel=""Info"" writeTo=""defaultFile"" />
  </rules>
</nlog>";

}
