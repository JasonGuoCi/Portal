

namespace Envision.SPS.Utility.Enums
{
    public enum MethodName
    {
        GetLeftMenus = 0,
        AddDocumentLibrary = 1,
        getUserGroups = 2,
        IsExistDocumentLibrary = 3,
        addSPGroups = 4,
        getSPGroupsInfoByName = 5,
        editSPGroups = 6,
        getDocumentLibrary = 7,
        editDocumentLibrary = 8,
        IsExistSPGroups = 9,
        isExistEditDocumentLibrary = 10,
        GetTreeNodes = 11,
        GetTreeChildNodes = 12,
        GetItTreeNodes = 13,
        GetItTreeChildNodes = 14,
        addOwnersToGroup = 15,
        permissionsReportResult = 16,
        IsExistEditSPGroups = 17
    }

    public enum SPGroupRolesCategory
    {
        View = 1,//仅查看
        Read = 2,//只读
        Edit = 3,//编辑
        FullControl = 4//完全控制
    }

    public enum SPDocumentVersionControlCategory
    {
        None = 0,//空
        Main = 1,//主版本
        Minor = 2//次版本
    }

    public enum SPForceCheckout
    {
        是 = 1,
        否 = 0
    }

}
