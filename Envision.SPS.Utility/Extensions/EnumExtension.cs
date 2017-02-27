using Envision.SPS.Utility.Enums;
namespace Envision.SPS.Utility.Extensions
{
    public static class EnumExtension
    {
        public static string ToDisplayName(this LogType logType)
        {
            switch (logType)
            {
                case LogType.AddFolder:
                    return "新增文件夹";
                case LogType.EditFolder:
                    return "编辑文件夹";
                case LogType.ViewFolder:
                    return "查看文件夹";
                case LogType.DownloadFolder:
                    return "下载文件夹";
                case LogType.RemoveFolder:
                    return "删除文件夹";
                case LogType.MoveFolder:
                    return "移动文件夹";
                case LogType.CopyFolder:
                    return "复制文件夹";
                case LogType.AddDocumentSet:
                    return "新增文档集";
                case LogType.EditDocumentSet:
                    return "编辑文档集";
                case LogType.ViewDocumentSet:
                    return "查看文档集";
                case LogType.DownloadDocumentSet:
                    return "下载文档集";
                case LogType.RemoveDocumentSet:
                    return "删除文档集";
                case LogType.MoveDocumentSet:
                    return "移动文档集";
                case LogType.CopyDocumentSet:
                    return "复制文档集";
                case LogType.AddDocument:
                    return "新增文档";
                case LogType.EditDocument:
                    return "编辑文档";
                case LogType.ViewDocument:
                    return "查看文档";
                case LogType.RemoveDocument:
                    return "删除文档";
                case LogType.DownloadDocument:
                    return "下载文档";
                case LogType.MoveDocument:
                    return "移动文档";
                case LogType.CopyDocument:
                    return "复制文档";
                case LogType.OpenDocument:
                    return "打开文档";
                case LogType.OnlineViewDocument:
                    return "在线打开";
                case LogType.OnlineEditDocument:
                    return "在线编辑";
                case LogType.CheckInDocument:
                    return "签入";
                case LogType.CheckOutDocument:
                    return "签出";
                case LogType.UndoCheckOutDocument:
                    return "撤销签出";
                case LogType.RemoveDocumentVersion:
                    return "删除历史版本";
                case LogType.ViewDocumentVersions:
                    return "查看历史版本";
                case LogType.ViewDocumentVersion:
                    return "查看文档版本";
                case LogType.RetrieveDocumentVersion:
                    return "还原文档版本";
                default:
                    return "";
            }
        }
    }
}
