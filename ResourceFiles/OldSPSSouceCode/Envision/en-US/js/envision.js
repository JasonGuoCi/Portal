

if (typeof (EnvisionJS) == "undefined") {
    var EnvisionJS = {}
}

EnvisionJS.Translation = {
    Init: function () {
    	$.getJSON("http://sp.envisioncn.com:8080/_api/web/lists/getByTitle('Translation')/items", function(data) {
				$.each(data.value, function(k, v) {
					var ChineseText = data.value[k].Chinese;
                    var EnglishText = data.value[k].Title;
                    var selector = data.value[k].Selector;
                    var lcid = _spPageContextInfo.currentLanguage;
                    if(lcid == 2052){
	                    $(selector ).each(function () {
	                            if ($(this).text() == EnglishText) {
	                                $(this).text(ChineseText);
	                            }
	                        });                        
						
					}	
							
			})   
		});     
    } 
}


ExecuteOrDelayUntilScriptLoaded(EnvisionJS.Translation.Init, "sp.js");
//SP.SOD.executeFunc('sp.js', 'SP.ClientContext', EnvisionJS.Translation.Init);
