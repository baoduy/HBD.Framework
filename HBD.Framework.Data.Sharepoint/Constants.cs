using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Data.Sharepoint
{
    public class SPStaticProperties
    {
        public class DefaultInternalFieldNames
        {
            public const string ID = "ID";
            public const string Title = "Tiltle";
        }

        public class UserInternalFieldNames : DefaultInternalFieldNames
        {
            public const string LoginName = "LoginName";
            public const string Email = "Email";
            public const string PrincipalType = "PrincipalType";
        }
    }
    public static class DefaultSPValues
    {
        public static class InternalFieldName
        {
            public const string ID = "ID";
            public const string ContentTypeId = "ContentTypeId";
            public const string ContentType = "ContentType";
            public const string Created = "Created";
            public const string Author = "Author";
            public const string Modified = "Modified";
            public const string Editor = "Editor";
            public const string HasCopyDestinations = "_HasCopyDestinations";
            public const string CopySource = "_CopySource";
            public const string ModerationStatus = "_ModerationStatus";
            public const string ModerationComments = "_ModerationComments";
            public const string FileRef = "FileRef";
            public const string FileDirRef = "FileDirRef";
            public const string Last_x0020_Modified = "Last_x0020_Modified";
            public const string Created_x0020_Date = "Created_x0020_Date";
            public const string File_x0020_Size = "File_x0020_Size";
            public const string FSObjType = "FSObjType";
            public const string PermMask = "PermMask";
            public const string CheckedOutUserId = "CheckedOutUserId";
            public const string IsCheckedoutToLocal = "IsCheckedoutToLocal";
            public const string CheckoutUser = "CheckoutUser";
            public const string FileLeafRef = "FileLeafRef";
            public const string UniqueId = "UniqueId";
            public const string ProgId = "ProgId";
            public const string ScopeId = "ScopeId";
            public const string VirusStatus = "VirusStatus";
            public const string CheckedOutTitle = "CheckedOutTitle";
            public const string CheckinComment = "_CheckinComment";
            public const string LinkCheckedOutTitle = "LinkCheckedOutTitle";
            public const string Modified_x0020_By = "Modified_x0020_By";
            public const string Created_x0020_By = "Created_x0020_By";
            public const string File_x0020_Type = "File_x0020_Type";
            public const string HTML_x0020_File_x0020_Type = "HTML_x0020_File_x0020_Type";
            public const string SourceUrl = "_SourceUrl";
            public const string SharedFileIndex = "_SharedFileIndex";
            public const string EditMenuTableStart = "_EditMenuTableStart";
            public const string EditMenuTableEnd = "_EditMenuTableEnd";
            public const string LinkFilenameNoMenu = "LinkFilenameNoMenu";
            public const string LinkFilename = "LinkFilename";
            public const string DocIcon = "DocIcon";
            public const string ServerUrl = "ServerUrl";
            public const string EncodedAbsUrl = "EncodedAbsUrl";
            public const string BaseName = "BaseName";
            public const string FileSizeDisplay = "FileSizeDisplay";
            public const string MetaInfo = "MetaInfo";
            public const string Level = "_Level";
            public const string IsCurrentVersion = "_IsCurrentVersion";
            public const string SelectTitle = "SelectTitle";
            public const string SelectFilename = "SelectFilename";
            public const string Edit = "Edit";
            public const string owshiddenversion = "owshiddenversion";
            public const string UIVersion = "_UIVersion";
            public const string UIVersionString = "_UIVersionString";
            public const string InstanceID = "InstanceID";
            public const string Order = "Order";
            public const string GUID = "GUID";
            public const string WorkflowVersion = "WorkflowVersion";
            public const string WorkflowInstanceID = "WorkflowInstanceID";
            public const string ParentVersionString = "ParentVersionString";
            public const string ParentLeafName = "ParentLeafName";
            public const string Title = "Title";
            public const string TemplateUrl = "TemplateUrl";
            public const string xd_ProgID = "xd_ProgID";
            public const string xd_Signature = "xd_Signature";
            public const string Combine = "Combine";
            public const string RepairDocument = "RepairDocument";
            public const string Attachments = "Attachments";
            public const string LinkTitleNoMenu = "LinkTitleNoMenu";
            public const string LinkTitle = "LinkTitle";
        }
    }
}
