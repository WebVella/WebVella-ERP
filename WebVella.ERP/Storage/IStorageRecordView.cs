using System;
using System.Collections.Generic;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
    //public interface IStorageRecordViewField
    //{
    //    Guid Id { get; set; }

    //    Guid EntityId { get; set; }

    //    RecordViewColumns Column { get; set; }

    //    int Position { get; set; }
    //}

    //public interface IStorageRecordView
    //{
    //    Guid Id { get; set; }

    //    string Name { get; set; }

    //    string Label { get; set; }

    //    IList<IStorageRecordViewField> Fields { get; set; }
    //}




    public interface IStorageRecordView
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Label { get; set; }

        bool Default { get; set; }

        bool System { get; set; }

        decimal? Weight { get; set; }

        string CssClass { get; set; }

        RecordViewType Type { get; set; }

        List<IStorageRecordViewRegion> Regions { get; set; }

        IStorageRecordViewSidebar Sidebar { get; set; }

    }

    ////////////////////////
    public interface IStorageRecordViewSidebar
    {
        bool Render { get; set; }

        string CssClass { get; set; }

        List<IStorageRecordViewSidebarList> Lists { get; set; }
    }

    ////////////////////////
    public interface IStorageRecordViewSidebarList
    {
        Guid EntityId { get; set; }

        Guid ListId { get; set; }

        Guid RelationId { get; set; }
    }

    ////////////////////////
    public interface IStorageRecordViewRegion
    {
        string Name { get; set; }

        bool Render { get; set; }

        string CssClass { get; set; }

        List<IStorageRecordViewSection> Sections { get; set; }
    }

    ////////////////////////
    public class IStorageRecordViewSection
    {
		Guid Id { get; set; }

		string Name { get; set; }

        string Label { get; set; }

        string CssClass { get; set; }

        bool ShowLabel { get; set; }

        bool Collapsed { get; set; }

        decimal? Weight { get; set; }

        string TabOrder { get; set; }

        List<IStorageRecordViewRow> Rows { get; set; }

    }

    ////////////////////////
    public interface IStorageRecordViewRow
    {
		Guid Id { get; set; }

		decimal? Weight { get; set; }

        List<IStorageRecordViewColumn> Columns { get; set; }
    }

    ////////////////////////
    public interface IStorageRecordViewColumn
    {
        List<IStorageRecordViewItemBase> Items { get; set; }

		int GridColCount { get; set; }
	}

    ////////////////////////
    public interface IStorageRecordViewItemBase
    {
    }

    public interface IStorageRecordViewFieldItem : IStorageRecordViewItemBase
    {
        Guid FieldId { get; set; }
    }

    public interface IStorageRecordViewListItem : IStorageRecordViewItemBase
    {
        Guid ListId { get; set; }
    }

    public interface IStorageRecordViewViewItem : IStorageRecordViewItemBase
    {
        Guid ViewId { get; set; }
    }

    public interface IStorageRecordViewHtmlItem : IStorageRecordViewItemBase
    {
        string Tag { get; set; }

        string Content { get; set; }
    }

    public interface IStorageRecordViewRelationFieldItem : IStorageRecordViewItemBase
    {
        Guid RelationId { get; set; }

        Guid FieldId { get; set; }
    }
}


