﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34209
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Envision.SPS.DataAccess
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SPSEnvision")]
	public partial class EFDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertSPS_Storage(SPS_Storage instance);
    partial void UpdateSPS_Storage(SPS_Storage instance);
    partial void DeleteSPS_Storage(SPS_Storage instance);
    partial void InsertSPS_EventBus(SPS_EventBus instance);
    partial void UpdateSPS_EventBus(SPS_EventBus instance);
    partial void DeleteSPS_EventBus(SPS_EventBus instance);
    #endregion
		
		public EFDataContext() :
        base(global::System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public EFDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EFDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EFDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EFDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SPS_Storage> SPS_Storage
		{
			get
			{
				return this.GetTable<SPS_Storage>();
			}
		}
		
		public System.Data.Linq.Table<SPS_EventBus> SPS_EventBus
		{
			get
			{
				return this.GetTable<SPS_EventBus>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_SPS_Storage_QueryList")]
		public ISingleResult<SP_SPS_Storage_QueryListResult> SP_SPS_Storage_QueryList([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ListName", DbType="NVarChar(MAX)")] string listName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), listName);
			return ((ISingleResult<SP_SPS_Storage_QueryListResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SPS_Storage")]
	public partial class SPS_Storage : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _WebName;
		
		private string _WebID;
		
		private string _WebUrl;
		
		private string _ListName;
		
		private string _ListID;
		
		private string _ListUrl;
		
		private System.Nullable<int> _FolderNumber;
		
		private System.Nullable<int> _FileNumber;
		
		private System.Nullable<decimal> _Storage;
		
		private string _Owners;
		
		private System.Nullable<int> _DesitionType;
		
		private System.Nullable<System.DateTime> _Created;
		
		private string _CreatorAccount;
		
		private string _CreatorUserName;
		
		private string _ParentWebID;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnWebNameChanging(string value);
    partial void OnWebNameChanged();
    partial void OnWebIDChanging(string value);
    partial void OnWebIDChanged();
    partial void OnWebUrlChanging(string value);
    partial void OnWebUrlChanged();
    partial void OnListNameChanging(string value);
    partial void OnListNameChanged();
    partial void OnListIDChanging(string value);
    partial void OnListIDChanged();
    partial void OnListUrlChanging(string value);
    partial void OnListUrlChanged();
    partial void OnFolderNumberChanging(System.Nullable<int> value);
    partial void OnFolderNumberChanged();
    partial void OnFileNumberChanging(System.Nullable<int> value);
    partial void OnFileNumberChanged();
    partial void OnStorageChanging(System.Nullable<decimal> value);
    partial void OnStorageChanged();
    partial void OnOwnersChanging(string value);
    partial void OnOwnersChanged();
    partial void OnDesitionTypeChanging(System.Nullable<int> value);
    partial void OnDesitionTypeChanged();
    partial void OnCreatedChanging(System.Nullable<System.DateTime> value);
    partial void OnCreatedChanged();
    partial void OnCreatorAccountChanging(string value);
    partial void OnCreatorAccountChanged();
    partial void OnCreatorUserNameChanging(string value);
    partial void OnCreatorUserNameChanged();
    partial void OnParentWebIDChanging(string value);
    partial void OnParentWebIDChanged();
    #endregion
		
		public SPS_Storage()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WebName", DbType="NVarChar(200)")]
		public string WebName
		{
			get
			{
				return this._WebName;
			}
			set
			{
				if ((this._WebName != value))
				{
					this.OnWebNameChanging(value);
					this.SendPropertyChanging();
					this._WebName = value;
					this.SendPropertyChanged("WebName");
					this.OnWebNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WebID", DbType="NVarChar(50)")]
		public string WebID
		{
			get
			{
				return this._WebID;
			}
			set
			{
				if ((this._WebID != value))
				{
					this.OnWebIDChanging(value);
					this.SendPropertyChanging();
					this._WebID = value;
					this.SendPropertyChanged("WebID");
					this.OnWebIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WebUrl", DbType="NVarChar(500)")]
		public string WebUrl
		{
			get
			{
				return this._WebUrl;
			}
			set
			{
				if ((this._WebUrl != value))
				{
					this.OnWebUrlChanging(value);
					this.SendPropertyChanging();
					this._WebUrl = value;
					this.SendPropertyChanged("WebUrl");
					this.OnWebUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListName", DbType="NVarChar(50)")]
		public string ListName
		{
			get
			{
				return this._ListName;
			}
			set
			{
				if ((this._ListName != value))
				{
					this.OnListNameChanging(value);
					this.SendPropertyChanging();
					this._ListName = value;
					this.SendPropertyChanged("ListName");
					this.OnListNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListID", DbType="NVarChar(50)")]
		public string ListID
		{
			get
			{
				return this._ListID;
			}
			set
			{
				if ((this._ListID != value))
				{
					this.OnListIDChanging(value);
					this.SendPropertyChanging();
					this._ListID = value;
					this.SendPropertyChanged("ListID");
					this.OnListIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListUrl", DbType="NVarChar(500)")]
		public string ListUrl
		{
			get
			{
				return this._ListUrl;
			}
			set
			{
				if ((this._ListUrl != value))
				{
					this.OnListUrlChanging(value);
					this.SendPropertyChanging();
					this._ListUrl = value;
					this.SendPropertyChanged("ListUrl");
					this.OnListUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FolderNumber", DbType="Int")]
		public System.Nullable<int> FolderNumber
		{
			get
			{
				return this._FolderNumber;
			}
			set
			{
				if ((this._FolderNumber != value))
				{
					this.OnFolderNumberChanging(value);
					this.SendPropertyChanging();
					this._FolderNumber = value;
					this.SendPropertyChanged("FolderNumber");
					this.OnFolderNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FileNumber", DbType="Int")]
		public System.Nullable<int> FileNumber
		{
			get
			{
				return this._FileNumber;
			}
			set
			{
				if ((this._FileNumber != value))
				{
					this.OnFileNumberChanging(value);
					this.SendPropertyChanging();
					this._FileNumber = value;
					this.SendPropertyChanged("FileNumber");
					this.OnFileNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Storage", DbType="Decimal(18,2)")]
		public System.Nullable<decimal> Storage
		{
			get
			{
				return this._Storage;
			}
			set
			{
				if ((this._Storage != value))
				{
					this.OnStorageChanging(value);
					this.SendPropertyChanging();
					this._Storage = value;
					this.SendPropertyChanged("Storage");
					this.OnStorageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Owners", DbType="NVarChar(500)")]
		public string Owners
		{
			get
			{
				return this._Owners;
			}
			set
			{
				if ((this._Owners != value))
				{
					this.OnOwnersChanging(value);
					this.SendPropertyChanging();
					this._Owners = value;
					this.SendPropertyChanged("Owners");
					this.OnOwnersChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DesitionType", DbType="Int")]
		public System.Nullable<int> DesitionType
		{
			get
			{
				return this._DesitionType;
			}
			set
			{
				if ((this._DesitionType != value))
				{
					this.OnDesitionTypeChanging(value);
					this.SendPropertyChanging();
					this._DesitionType = value;
					this.SendPropertyChanged("DesitionType");
					this.OnDesitionTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Created", DbType="DateTime")]
		public System.Nullable<System.DateTime> Created
		{
			get
			{
				return this._Created;
			}
			set
			{
				if ((this._Created != value))
				{
					this.OnCreatedChanging(value);
					this.SendPropertyChanging();
					this._Created = value;
					this.SendPropertyChanged("Created");
					this.OnCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatorAccount", DbType="NVarChar(50)")]
		public string CreatorAccount
		{
			get
			{
				return this._CreatorAccount;
			}
			set
			{
				if ((this._CreatorAccount != value))
				{
					this.OnCreatorAccountChanging(value);
					this.SendPropertyChanging();
					this._CreatorAccount = value;
					this.SendPropertyChanged("CreatorAccount");
					this.OnCreatorAccountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatorUserName", DbType="NVarChar(50)")]
		public string CreatorUserName
		{
			get
			{
				return this._CreatorUserName;
			}
			set
			{
				if ((this._CreatorUserName != value))
				{
					this.OnCreatorUserNameChanging(value);
					this.SendPropertyChanging();
					this._CreatorUserName = value;
					this.SendPropertyChanged("CreatorUserName");
					this.OnCreatorUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentWebID", DbType="NVarChar(50)")]
		public string ParentWebID
		{
			get
			{
				return this._ParentWebID;
			}
			set
			{
				if ((this._ParentWebID != value))
				{
					this.OnParentWebIDChanging(value);
					this.SendPropertyChanging();
					this._ParentWebID = value;
					this.SendPropertyChanged("ParentWebID");
					this.OnParentWebIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SPS_EventBus")]
	public partial class SPS_EventBus : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _SiteID;
		
		private string _WebID;
		
		private string _ListID;
		
		private string _ItemID;
		
		private string _EventName;
		
		private System.Nullable<System.DateTime> _EventTime;
		
		private string _UserName;
		
		private string _UserID;
		
		private string _UserEmail;
		
		private System.Nullable<byte> _Status;
		
		private System.Nullable<bool> _IsEmail;
		
		private string _FileName;
		
		private string _FilePath;
		
		private System.Nullable<System.DateTime> _CompletedTime;
		
		private System.Nullable<System.DateTime> _CreatedTime;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnSiteIDChanging(string value);
    partial void OnSiteIDChanged();
    partial void OnWebIDChanging(string value);
    partial void OnWebIDChanged();
    partial void OnListIDChanging(string value);
    partial void OnListIDChanged();
    partial void OnItemIDChanging(string value);
    partial void OnItemIDChanged();
    partial void OnEventNameChanging(string value);
    partial void OnEventNameChanged();
    partial void OnEventTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnEventTimeChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnUserIDChanging(string value);
    partial void OnUserIDChanged();
    partial void OnUserEmailChanging(string value);
    partial void OnUserEmailChanged();
    partial void OnStatusChanging(System.Nullable<byte> value);
    partial void OnStatusChanged();
    partial void OnIsEmailChanging(System.Nullable<bool> value);
    partial void OnIsEmailChanged();
    partial void OnFileNameChanging(string value);
    partial void OnFileNameChanged();
    partial void OnFilePathChanging(string value);
    partial void OnFilePathChanged();
    partial void OnCompletedTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnCompletedTimeChanged();
    partial void OnCreatedTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnCreatedTimeChanged();
    #endregion
		
		public SPS_EventBus()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SiteID", DbType="NVarChar(50)")]
		public string SiteID
		{
			get
			{
				return this._SiteID;
			}
			set
			{
				if ((this._SiteID != value))
				{
					this.OnSiteIDChanging(value);
					this.SendPropertyChanging();
					this._SiteID = value;
					this.SendPropertyChanged("SiteID");
					this.OnSiteIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WebID", DbType="NVarChar(50)")]
		public string WebID
		{
			get
			{
				return this._WebID;
			}
			set
			{
				if ((this._WebID != value))
				{
					this.OnWebIDChanging(value);
					this.SendPropertyChanging();
					this._WebID = value;
					this.SendPropertyChanged("WebID");
					this.OnWebIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListID", DbType="NVarChar(MAX)")]
		public string ListID
		{
			get
			{
				return this._ListID;
			}
			set
			{
				if ((this._ListID != value))
				{
					this.OnListIDChanging(value);
					this.SendPropertyChanging();
					this._ListID = value;
					this.SendPropertyChanged("ListID");
					this.OnListIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemID", DbType="NVarChar(50)")]
		public string ItemID
		{
			get
			{
				return this._ItemID;
			}
			set
			{
				if ((this._ItemID != value))
				{
					this.OnItemIDChanging(value);
					this.SendPropertyChanging();
					this._ItemID = value;
					this.SendPropertyChanged("ItemID");
					this.OnItemIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EventName", DbType="NVarChar(50)")]
		public string EventName
		{
			get
			{
				return this._EventName;
			}
			set
			{
				if ((this._EventName != value))
				{
					this.OnEventNameChanging(value);
					this.SendPropertyChanging();
					this._EventName = value;
					this.SendPropertyChanged("EventName");
					this.OnEventNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EventTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> EventTime
		{
			get
			{
				return this._EventTime;
			}
			set
			{
				if ((this._EventTime != value))
				{
					this.OnEventTimeChanging(value);
					this.SendPropertyChanging();
					this._EventTime = value;
					this.SendPropertyChanged("EventTime");
					this.OnEventTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(50)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="NVarChar(50)")]
		public string UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._UserID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserEmail", DbType="NVarChar(50)")]
		public string UserEmail
		{
			get
			{
				return this._UserEmail;
			}
			set
			{
				if ((this._UserEmail != value))
				{
					this.OnUserEmailChanging(value);
					this.SendPropertyChanging();
					this._UserEmail = value;
					this.SendPropertyChanged("UserEmail");
					this.OnUserEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="TinyInt")]
		public System.Nullable<byte> Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsEmail", DbType="Bit")]
		public System.Nullable<bool> IsEmail
		{
			get
			{
				return this._IsEmail;
			}
			set
			{
				if ((this._IsEmail != value))
				{
					this.OnIsEmailChanging(value);
					this.SendPropertyChanging();
					this._IsEmail = value;
					this.SendPropertyChanged("IsEmail");
					this.OnIsEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FileName", DbType="NVarChar(MAX)")]
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if ((this._FileName != value))
				{
					this.OnFileNameChanging(value);
					this.SendPropertyChanging();
					this._FileName = value;
					this.SendPropertyChanged("FileName");
					this.OnFileNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FilePath", DbType="NVarChar(1000)")]
		public string FilePath
		{
			get
			{
				return this._FilePath;
			}
			set
			{
				if ((this._FilePath != value))
				{
					this.OnFilePathChanging(value);
					this.SendPropertyChanging();
					this._FilePath = value;
					this.SendPropertyChanged("FilePath");
					this.OnFilePathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CompletedTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> CompletedTime
		{
			get
			{
				return this._CompletedTime;
			}
			set
			{
				if ((this._CompletedTime != value))
				{
					this.OnCompletedTimeChanging(value);
					this.SendPropertyChanging();
					this._CompletedTime = value;
					this.SendPropertyChanged("CompletedTime");
					this.OnCompletedTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> CreatedTime
		{
			get
			{
				return this._CreatedTime;
			}
			set
			{
				if ((this._CreatedTime != value))
				{
					this.OnCreatedTimeChanging(value);
					this.SendPropertyChanging();
					this._CreatedTime = value;
					this.SendPropertyChanged("CreatedTime");
					this.OnCreatedTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class SP_SPS_Storage_QueryListResult
	{
		
		private long _ID;
		
		private string _WebName;
		
		private string _WebID;
		
		private string _WebUrl;
		
		private string _ListName;
		
		private string _ListID;
		
		private string _ListUrl;
		
		private System.Nullable<int> _FolderNumber;
		
		private System.Nullable<int> _FileNumber;
		
		private System.Nullable<decimal> _Storage;
		
		private string _Owners;
		
		private System.Nullable<int> _DesitionType;
		
		private System.Nullable<System.DateTime> _Created;
		
		private string _CreatorAccount;
		
		private string _CreatorUserName;
		
		private string _ParentWebID;
		
		public SP_SPS_Storage_QueryListResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", DbType="BigInt NOT NULL")]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this._ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WebName", DbType="NVarChar(200)")]
		public string WebName
		{
			get
			{
				return this._WebName;
			}
			set
			{
				if ((this._WebName != value))
				{
					this._WebName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WebID", DbType="NVarChar(50)")]
		public string WebID
		{
			get
			{
				return this._WebID;
			}
			set
			{
				if ((this._WebID != value))
				{
					this._WebID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WebUrl", DbType="NVarChar(500)")]
		public string WebUrl
		{
			get
			{
				return this._WebUrl;
			}
			set
			{
				if ((this._WebUrl != value))
				{
					this._WebUrl = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListName", DbType="Char(10)")]
		public string ListName
		{
			get
			{
				return this._ListName;
			}
			set
			{
				if ((this._ListName != value))
				{
					this._ListName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListID", DbType="NVarChar(50)")]
		public string ListID
		{
			get
			{
				return this._ListID;
			}
			set
			{
				if ((this._ListID != value))
				{
					this._ListID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListUrl", DbType="NVarChar(500)")]
		public string ListUrl
		{
			get
			{
				return this._ListUrl;
			}
			set
			{
				if ((this._ListUrl != value))
				{
					this._ListUrl = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FolderNumber", DbType="Int")]
		public System.Nullable<int> FolderNumber
		{
			get
			{
				return this._FolderNumber;
			}
			set
			{
				if ((this._FolderNumber != value))
				{
					this._FolderNumber = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FileNumber", DbType="Int")]
		public System.Nullable<int> FileNumber
		{
			get
			{
				return this._FileNumber;
			}
			set
			{
				if ((this._FileNumber != value))
				{
					this._FileNumber = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Storage", DbType="Decimal(18,2)")]
		public System.Nullable<decimal> Storage
		{
			get
			{
				return this._Storage;
			}
			set
			{
				if ((this._Storage != value))
				{
					this._Storage = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Owners", DbType="NVarChar(500)")]
		public string Owners
		{
			get
			{
				return this._Owners;
			}
			set
			{
				if ((this._Owners != value))
				{
					this._Owners = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DesitionType", DbType="Int")]
		public System.Nullable<int> DesitionType
		{
			get
			{
				return this._DesitionType;
			}
			set
			{
				if ((this._DesitionType != value))
				{
					this._DesitionType = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Created", DbType="DateTime")]
		public System.Nullable<System.DateTime> Created
		{
			get
			{
				return this._Created;
			}
			set
			{
				if ((this._Created != value))
				{
					this._Created = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatorAccount", DbType="NVarChar(50)")]
		public string CreatorAccount
		{
			get
			{
				return this._CreatorAccount;
			}
			set
			{
				if ((this._CreatorAccount != value))
				{
					this._CreatorAccount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatorUserName", DbType="NVarChar(50)")]
		public string CreatorUserName
		{
			get
			{
				return this._CreatorUserName;
			}
			set
			{
				if ((this._CreatorUserName != value))
				{
					this._CreatorUserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentWebID", DbType="NVarChar(50)")]
		public string ParentWebID
		{
			get
			{
				return this._ParentWebID;
			}
			set
			{
				if ((this._ParentWebID != value))
				{
					this._ParentWebID = value;
				}
			}
		}
	}
}
#pragma warning restore 1591