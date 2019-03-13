function WvSectionCallApiCollapseToggle(nodeId,isCollapsed) {
		$.ajax({
            type: "POST",
            url: '/api/v3.0/user/preferences/toggle-section-collapse?nodeId=' + nodeId + "&isCollapsed=" + isCollapsed,
			data: $(event.target).serialize(),
			success: function (response) {
				if (!response.success) {
                    console.error(response.message);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
                console.error(jqXHR);
                console.error(textStatus);
                console.error(errorThrown);
			}
		});
}


function WvSectionInit(sectionId) {
    var collapseId = "#collapse-" + sectionId;
    var nodeId = sectionId.replace("wv-", "");
    $(collapseId).on('show.bs.collapse', function () {
        WvSectionCallApiCollapseToggle(nodeId,false);
    });
    $(collapseId).on('hide.bs.collapse', function () {
        WvSectionCallApiCollapseToggle(nodeId,true);
    });
}