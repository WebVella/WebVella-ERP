function WvSectionCallApiCollapseToggle(nodeId, isCollapsed) {
    var pattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    if (pattern.test(nodeId) === true) {
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
}


function WvSectionInit(sectionId) {
    var collapseId = "#collapse-" + sectionId;
    var nodeId = sectionId.replace("wv-", "");
    $(".lns-header").click(function (event) {
        if (event.target.className === this.className) {
            event.preventDefault();
            //event.stopPropagation();
            var linkEl = $(this).find("a[data-toggle]");
            if (linkEl) {
                $(linkEl).trigger('click');
            }
        }
    });


    $(collapseId).on('show.bs.collapse', function () {
        WvSectionCallApiCollapseToggle(nodeId, false);
    });
    $(collapseId).on('hide.bs.collapse', function () {
        WvSectionCallApiCollapseToggle(nodeId, true);
    });
}