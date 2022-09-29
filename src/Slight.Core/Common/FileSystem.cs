using System;
using System.IO;
using System.Linq;

namespace Slight.Core
{
    public static class FileSystem
    {
        public static readonly CurrentFolder Current = new();
        public static readonly DesktopFolder Desktop = new();
        public static readonly ProgramsFolder Programs = new();
        public static readonly MyDocumentsFolder MyDocuments = new();
        public static readonly PersonalFolder Personal = new();
        public static readonly FavoritesFolder Favorites = new();
        public static readonly StartupFolder Startup = new();
        public static readonly RecentFolder Recent = new();
        public static readonly SendToFolder SendTo = new();
        public static readonly StartMenuFolder StartMenu = new();
        public static readonly MyMusicFolder MyMusic = new();
        public static readonly MyVideosFolder MyVideos = new();
        public static readonly DesktopDirectoryFolder DesktopDirectory = new();
        public static readonly MyComputerFolder MyComputer = new();
        public static readonly NetworkShortcutsFolder NetworkShortcuts = new();
        public static readonly FontsFolder Fonts = new();
        public static readonly TemplatesFolder Templates = new();
        public static readonly CommonStartMenuFolder CommonStartMenu = new();
        public static readonly CommonProgramsFolder CommonPrograms = new();
        public static readonly CommonStartupFolder CommonStartup = new();
        public static readonly CommonDesktopDirectoryFolder CommonDesktopDirectory = new();
        public static readonly ApplicationDataFolder ApplicationData = new();
        public static readonly PrinterShortcutsFolder PrinterShortcuts = new();
        public static readonly LocalApplicationDataFolder LocalApplicationData = new();
        public static readonly InternetCacheFolder InternetCache = new();
        public static readonly CookiesFolder Cookies = new();
        public static readonly HistoryFolder History = new();
        public static readonly CommonApplicationDataFolder CommonApplicationData = new();
        public static readonly WindowsFolder Windows = new();
        public static readonly SystemFolder System = new();
        public static readonly ProgramFilesFolder ProgramFiles = new();
        public static readonly MyPicturesFolder MyPictures = new();
        public static readonly UserProfileFolder UserProfile = new();
        public static readonly SystemX86Folder SystemX86 = new();
        public static readonly ProgramFilesX86Folder ProgramFilesX86 = new();
        public static readonly CommonProgramFilesFolder CommonProgramFiles = new();
        public static readonly CommonProgramFilesX86Folder CommonProgramFilesX86 = new();
        public static readonly CommonTemplatesFolder CommonTemplates = new();
        public static readonly CommonDocumentsFolder CommonDocuments = new();
        public static readonly CommonAdminToolsFolder CommonAdminTools = new();
        public static readonly AdminToolsFolder AdminTools = new();
        public static readonly CommonMusicFolder CommonMusic = new();
        public static readonly CommonPicturesFolder CommonPictures = new();
        public static readonly CommonVideosFolder CommonVideos = new();
        public static readonly ResourcesFolder Resources = new();
        public static readonly LocalizedResourcesFolder LocalizedResources = new();
        public static readonly CommonOemLinksFolder CommonOemLinks = new();
        public static readonly CDBurningFolder CDBurning = new();


    }
    public readonly struct CurrentFolder
    {
        public override string ToString()
        {
            return Environment.CurrentDirectory;
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CurrentFolder currentFolder)
        {
            return currentFolder.ToString();
        }
    }
    public readonly struct DesktopFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(DesktopFolder desktopFolder)
        {
            return desktopFolder.ToString();
        }
    }
    public readonly struct ProgramsFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Programs);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray()); 
        }

        public static explicit operator string(ProgramsFolder programsFolder)
        {
            return programsFolder.ToString();
        }
    }
    public readonly struct MyDocumentsFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(MyDocumentsFolder myDocumentsFolder)
        {
            return myDocumentsFolder.ToString();
        }
    }
    public readonly struct PersonalFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(PersonalFolder personalFolder)
        {
            return personalFolder.ToString();
        }
    }
    public readonly struct FavoritesFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(FavoritesFolder favoritesFolder)
        {
            return favoritesFolder.ToString();
        }
    }
    public readonly struct StartupFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(StartupFolder startupFolder)
        {
            return startupFolder.ToString();
        }
    }
    public readonly struct RecentFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Recent);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(RecentFolder recentFolder)
        {
            return recentFolder.ToString();
        }
    }
    public readonly struct SendToFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.SendTo);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(SendToFolder sendToFolder)
        {
            return sendToFolder.ToString();
        }
    }
    public readonly struct StartMenuFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(StartMenuFolder startMenuFolder)
        {
            return startMenuFolder.ToString();
        }
    }
    public readonly struct MyMusicFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(MyMusicFolder myMusicFolder)
        {
            return myMusicFolder.ToString();
        }
    }
    public readonly struct MyVideosFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(MyVideosFolder myVideosFolder)
        {
            return myVideosFolder.ToString();
        }
    }
    public readonly struct DesktopDirectoryFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(DesktopDirectoryFolder desktopDirectoryFolder)
        {
            return desktopDirectoryFolder.ToString();
        }
    }
    public readonly struct MyComputerFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(MyComputerFolder myComputerFolder)
        {
            return myComputerFolder.ToString();
        }
    }
    public readonly struct NetworkShortcutsFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(NetworkShortcutsFolder networkShortcutsFolder)
        {
            return networkShortcutsFolder.ToString();
        }
    }
    public readonly struct FontsFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray()); 
        }

        public static explicit operator string(FontsFolder fontsFolder)
        {
            return fontsFolder.ToString();
        }
    }
    public readonly struct TemplatesFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(TemplatesFolder templatesFolder)
        {
            return templatesFolder.ToString();
        }
    }
    public readonly struct CommonStartMenuFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonStartMenuFolder commonStartMenuFolder)
        {
            return commonStartMenuFolder.ToString();
        }
    }
    public readonly struct CommonProgramsFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonProgramsFolder commonProgramsFolder)
        {
            return commonProgramsFolder.ToString();
        }
    }
    public readonly struct CommonStartupFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonStartupFolder commonStartupFolder)
        {
            return commonStartupFolder.ToString();
        }
    }
    public readonly struct CommonDesktopDirectoryFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonDesktopDirectoryFolder commonDesktopDirectoryFolder)
        {
            return commonDesktopDirectoryFolder.ToString();
        }
    }
    public readonly struct ApplicationDataFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(ApplicationDataFolder applicationDataFolder)
        {
            return applicationDataFolder.ToString();
        }
    }
    public readonly struct PrinterShortcutsFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.PrinterShortcuts);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());

           
        }

        public static explicit operator string(PrinterShortcutsFolder printerShortcutsFolder)
        {
            return printerShortcutsFolder.ToString();
        }
    }
    public readonly struct LocalApplicationDataFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(LocalApplicationDataFolder localApplicationDataFolder)
        {
            return localApplicationDataFolder.ToString();
        }
    }
    public readonly struct InternetCacheFolder
    {
         
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(InternetCacheFolder internetCacheFolder)
        {
            return internetCacheFolder.ToString();
        }
    }
    public readonly struct CookiesFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CookiesFolder cookiesFolder)
        {
            return cookiesFolder.ToString();
        }
    }
    public readonly struct HistoryFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.History);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(HistoryFolder historyFolder)
        {
            return historyFolder.ToString();
        }
    }
    public readonly struct CommonApplicationDataFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonApplicationDataFolder commonApplicationDataFolder)
        {
            return commonApplicationDataFolder.ToString();
        }
    }
    public readonly struct WindowsFolder
    {
         
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(WindowsFolder windowsFolder)
        {
            return windowsFolder.ToString();
        }
    }
    public readonly struct SystemFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.System);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(SystemFolder systemFolder)
        {
            return systemFolder.ToString();
        }
    }
    public readonly struct ProgramFilesFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(ProgramFilesFolder programFilesFolder)
        {
            return programFilesFolder.ToString();
        }
    }
    public readonly struct MyPicturesFolder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(MyPicturesFolder myPicturesFolder)
        {
            return myPicturesFolder.ToString();
        }
    }
    public readonly struct UserProfileFolder
    {
         
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(UserProfileFolder userProfileFolder)
        {
            return userProfileFolder.ToString();
        }
    }
    public readonly struct SystemX86Folder
    {
         
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(SystemX86Folder systemX86Folder)
        {
            return systemX86Folder.ToString();
        }
    }
    public readonly struct ProgramFilesX86Folder
    { 
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(ProgramFilesX86Folder programFilesX86Folder)
        {
            return programFilesX86Folder.ToString();
        }
    }
    public readonly struct CommonProgramFilesFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonProgramFilesFolder commonProgramFilesFolder)
        {
            return commonProgramFilesFolder.ToString();
        }
    }
    public readonly struct CommonProgramFilesX86Folder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonProgramFilesX86Folder commonProgramFilesX86Folder)
        {
            return commonProgramFilesX86Folder.ToString();
        }
    }
    public readonly struct CommonTemplatesFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonTemplates);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonTemplatesFolder commonTemplatesFolder)
        {
            return commonTemplatesFolder.ToString();
        }
    }
    public readonly struct CommonDocumentsFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonDocumentsFolder commonDocumentsFolder)
        {
            return commonDocumentsFolder.ToString();
        }
    }
    public readonly struct CommonAdminToolsFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonAdminTools);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonAdminToolsFolder commonAdminToolsFolder)
        {
            return commonAdminToolsFolder.ToString();
        }
    }
    public readonly struct AdminToolsFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.AdminTools);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(AdminToolsFolder adminToolsFolder)
        {
            return adminToolsFolder.ToString();
        }
    }
    public readonly struct CommonMusicFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonMusicFolder commonMusicFolder)
        {
            return commonMusicFolder.ToString();
        }
    }
    public readonly struct CommonPicturesFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonPicturesFolder commonPicturesFolder)
        {
            return commonPicturesFolder.ToString();
        }
    }
    public readonly struct CommonVideosFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonVideosFolder commonVideosFolder)
        {
            return commonVideosFolder.ToString();
        }
    }
    public readonly struct ResourcesFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Resources);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(ResourcesFolder resourcesFolder)
        {
            return resourcesFolder.ToString();
        }
    }
    public readonly struct LocalizedResourcesFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalizedResources);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(LocalizedResourcesFolder localizedResourcesFolder)
        {
            return localizedResourcesFolder.ToString();
        }
    }
    public readonly struct CommonOemLinksFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonOemLinks);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CommonOemLinksFolder commonOemLinksFolder)
        {
            return commonOemLinksFolder.ToString();
        }
    }
    public readonly struct CDBurningFolder
    {
        public override string ToString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CDBurning);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(new[] { ToString() }.Concat(paths).ToArray());
        }

        public static explicit operator string(CDBurningFolder cDBurningFolder)
        {
            return cDBurningFolder.ToString();
        }
    }
}
