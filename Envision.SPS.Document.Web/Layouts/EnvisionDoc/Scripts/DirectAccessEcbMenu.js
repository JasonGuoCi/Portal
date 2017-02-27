function AddDirectAccessEcbMenu() {
    if (!window.jQuery && !window.SmartJquery) {
        setTimeout(AddDirectAccessEcbMenu, 100);
        return;
    }
    var jq = (window.jQuery || window.SmartJquery);
    var fixedTable = null;
    jq(".ms-lstItmLinkAnchor").each(function () {
        var anchor = jq(this);
        var el = anchor.get(0);
        if (el.onclick && el.onclick.toString().indexOf("OpenCalloutAndSelectItem") >= 0 && !el.atemvisited) {
            el.atemvisited = true;
            var tr = anchor.closest("tr");
            var td = anchor.closest("td");
            var alinkline = td.find("div");
            var img = anchor.find("img");
            var trid = tr.attr("id");
            if (trid) {
                var values = trid.split(',');
                var alt = img.attr("alt");
                var open = jq('<td class="' + td.attr("class") + '"><span class="js-callout-ecbMenu" id="' + values[1] + '" eventtype="" perm="0x7fffffffffffffff" field="LinkFilename" ctxname="ctx' + values[0] + '"><a style="margin-top:6px;margin-left:0px;" onclick="HandleDocumentBodyClick();calloutCreateAjaxMenu(event); return false;" href="#" ms-jsgrid-click-passthrough="true" class="ms-lstItmLinkAnchor ms-ellipsis-a js-callout-action ms-calloutLinkEnabled ms-calloutLink js-ellipsis25-a"><img alt="' + alt + '" class="ms-ellipsis-icon" src="/_layouts/15/images/spcommon.png?rev=23" ></a></span></td>');
                img.remove();
                alinkline.remove();
                open.insertAfter(td);
                var table = tr.closest("table");
                var tableElement = table.get(0);
                if (fixedTable != tableElement) {
                    fixedTable = tableElement;
                    var index = td.index();
                    table.find("th:nth-child(" + index + ")").attr("colspan", 2);
                }
            }
        }
    });
}

function SmartJqueryInit() {
    if (!window.jQuery) {
        for (var prop in window) {
            var p = window[prop];
            if (p && p.fn && p.fn.jquery) {
                window.SmartJquery = p;
                break;
            }
        }
        if (!window.SmartJquery && !window._isSmartJqueryLoading) {
            window._isSmartJqueryLoading = true;
            var newscript = document.createElement('script');
            newscript.type = 'text/javascript';
            newscript.src = 'jquery.min.js';
            (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(newscript);
        }
    }
    else
        window.SmartJquery = jQuery;
}

function AlsoRunAfterMinimalDownload() {
    if (window.asyncDeltaManager && window.asyncDeltaManager.add_endRequest) {
        asyncDeltaManager.add_endRequest(function () {
            AddDirectAccessEcbMenu();
        });
    }
}

var decbOnPostRenderTabularListViewDelayedOriginal;
function DecbUploadAndSortFix() {
    if (window.OnPostRenderTabularListViewDelayed) {
        decbOnPostRenderTabularListViewDelayedOriginal = OnPostRenderTabularListViewDelayed;
        OnPostRenderTabularListViewDelayed = function (renderCtx) {
            AddDirectAccessEcbMenu();
            decbOnPostRenderTabularListViewDelayedOriginal(renderCtx);
        };
    }
}

_spBodyOnLoadFunctions.push(DecbUploadAndSortFix);
_spBodyOnLoadFunctions.push(SmartJqueryInit);
_spBodyOnLoadFunctions.push(AddDirectAccessEcbMenu);
_spBodyOnLoadFunctions.push(AlsoRunAfterMinimalDownload);

