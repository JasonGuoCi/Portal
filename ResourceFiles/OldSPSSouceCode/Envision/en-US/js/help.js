		
		function generateCategories() {
			$.getJSON("http://sp.envisioncn.com:8080/_api/web/lists/getByTitle('HomeContentCategory')/items", function(data) {
				$.each(data.value, function(k, v) {
					
					if (data.value[k].ContentAreaID === "1") {
						toAppend = $("#findItMyselfCategories");
					}
					if (data.value[k].ContentAreaID === "2") {
						toAppend = $("#askTheExpertsCategories");
					}
					
					var rawTemplate = $('#categoryTemplate').html();
					if(rawTemplate){
						var template = Handlebars.compile(rawTemplate);
						var htmlTemp = template(v);
						toAppend.append(htmlTemp);
					}
				});
			}).done(function() {
				
				//alert($("#findItMyselfCategories").width());
				//owlCarousel(owl);
				
				$(".owl-carousel").owlCarousel({
				pagination: false,
				items: 7,
				itemsDesktop: [1200, 7],
				//itemsMobile: [640, 5],
				//itemsTablet: [1000, 5],
				//itemsTabletSmall: [750, 4],
				navigation: true,
				navigationText: ["", ""],
				rewindNav: false
				});
				
				
				$("div.owl-wrapper > div.owl-item > div.item img").parent().attr("target", "_blank");
				
				// Bind Click Events
					$("#findItMyself .owl-item a").click(function() {
						
						var clickedCategory = $(this).closest('.content').attr('id');					
						if (clickedCategory == 'askTheExperts') {
							$('#findItMyselfResults').hide();
							$('#askTheExpertsResults').show();
	
						} else {
							$('#askTheExpertsResults').hide();
							$('#findItMyselfResults').show();
						}
						
					});

			});
		}
		
		function generateResults() {
		
			// Find It Myself Results
			var dataToParse = {};
			
			// Need to fetch some categorical data first
			$.getJSON("http://sp.envisioncn.com:8080/_api/web/lists/getByTitle('ITWebHomeContentSType')/items", function(catData) {
					
				// Then we fetch the actual items
				$.getJSON("http://sp.envisioncn.com:8080/_api/web/lists/getByTitle('ITWebHomeContent')/items?$select=Title,ContentName,DisplayContent,Category,ContentNameUIOrder,SType/RecordID,Category/RecordID&$expand=Category/RecordID,SType/RecordID&$filter=ContentAreaID eq '1'&$orderby=ContentNameUIOrder", function(data) {
								
									
					// Parse Data
					$.each(data.value, function(idx, item) {
						
						if (item.Category && item.SType) {
							category = item.SType.RecordID;
							record = item.Category.RecordID;
							order = 00;
							var index = null;
							for(var i = 0; i < catData.value.length; i++) {
							   if(catData.value[i].RecordID == category) {
							     index = i;
							   }
							}
							
							categoryHeader = catData.value[index].STypeDisplay;
							concatenated = category + ' ' + record;
							if (!dataToParse[concatenated]) {
								dataToParse[concatenated] = {
									title: categoryHeader,
									recordID: record,
									sort: category,
									items: []
								};
							}
							dataToParse[concatenated].items.push(item);
						} 					
					});
					
				}).done(function() {
				
					// Templatize and Append Results
					$.each(dataToParse, function() {
						var rawTemplate = $('#findItMyselfTemplate').html();
						var template = Handlebars.compile(rawTemplate);
						var toAppend = $("#findItMyselfResults");
						var htmlTemp = template(this);
						toAppend.prepend(htmlTemp);
												
						var count = 0;
						var updatedHTML = $('#findItMyselfResults .findItMyself-result').sort(function (a, b) {
						      var contentA =parseInt( $(a).attr('data-sort'));
						      var contentB =parseInt( $(b).attr('data-sort'));
						      return (contentA < contentB) ? -1 : (contentA > contentB) ? 1 : 0;
						   });
						$("#findItMyselfResults").html(updatedHTML);
					});
					
					// Bind Click Events
					$("#findItMyself .owl-item a").click(function() {
						
						var clickedCategory = $(this).closest('.content').attr('id');					
						if (clickedCategory == 'askTheExperts') {
							$('#findItMyselfResults').hide();
							$('#askTheExpertsResults').show();
	
						} else {
							$('#askTheExpertsResults').hide();
							$('#findItMyselfResults').show();
						}
						
						$(".owl-item a").parent().removeClass("selected");
						$(this).parent().addClass("selected");
						
						var showItemID = $(this).attr('data-record-id');
						$(".findItMyself-result").hide();
						$(".findItMyself-result[data-record-id="+showItemID+"]").show();
					});
				});
			});
			// Ask The Experts Results
			$.getJSON("http://sp.envisioncn.com:8080/_api/web/lists/getByTitle('ITWebHomeContent')/items?$select=Title,ContentName,DisplayContent,Category/RecordID&$expand=Category/RecordID&$filter=ContentAreaID eq '2'", function(data) {
				$.each(data.value, function(idx, item) {
					var rawTemplate = $('#askTheExpertsTemplate').html();
					var template = Handlebars.compile(rawTemplate);
					var toAppend = $("#askTheExpertsResults");
					var htmlTemp = template(item);
					toAppend.append(htmlTemp);
				});
			}).done(function() {
			
				// Bind Click Events
				$("#askTheExpertsCategories a").click(function() {
					
					var clickedCategory = $(this).closest('.content').attr('id');					
					if (clickedCategory == 'askTheExperts') {
						$('#findItMyselfResults').hide();
						$('#askTheExpertsResults').show();

					} else {
						$('#askTheExpertsResults').hide();
						$('#findItMyselfResults').show();
					}

					$(".owl-item a").parent().removeClass("selected");
					$(this).parent().addClass("selected");
					
					var showItemID = $(this).attr('data-record-id');
					$(".askTheExperts-result").hide();
					$(".askTheExperts-result[data-record-id="+showItemID+"]").show();
				});
				
				$(".tabs a, .owl-prev, .owl-next").click(function() {
					$(".askTheExperts-result,.findItMyself-result").hide();
				});
			});
			
		}
		
		
		function Init() {
			//$(document).foundation();
			generateCategories();				
			
			
			$(".tab-title").click(function(){
				//onclick for tabs, gets icon content depending on what tab text is
				if($(this).attr('id') == "tabAskTheExperts"){
					$("#tabFindItMyself").attr('class', 'tab-title');
					$("#tabAskTheExperts").attr('class', 'tab-title active');
					$("#tabGetAssistance").attr('class', 'tab-title');

					$("#findItMyself").hide();
					$("#askTheExperts").show();
					$("#getAssistance").hide();
				}
				else if($(this).attr('id') == "tabGetAssistance"){
					$("#tabFindItMyself").attr('class', 'tab-title');
					$("#tabAskTheExperts").attr('class', 'tab-title');
					$("#tabGetAssistance").attr('class', 'tab-title active');

					$("#findItMyself").hide();
					$("#askTheExperts").hide();
					$("#getAssistance").show();

				}
				else{
					$("#tabFindItMyself").attr('class', 'tab-title active');
					$("#tabAskTheExperts").attr('class', 'tab-title');
					$("#tabGetAssistance").attr('class', 'tab-title');

					$("#findItMyself").show();
					$("#askTheExperts").hide();
					$("#getAssistance").hide();

				}
			});
			
			$("#MiddleRow div.ms-webpart-zone div.ms-webpartzone-cell div.ms-wpContentDivSpace table.ms-listviewtable tr.ms-itmHoverEnabled div.ms-list-TitleLink>a.ms-listlink").attr("target", "_blank");
			$("#MiddleRow div.ms-webpart-zone div.ms-webpartzone-cell div.ms-wpContentDivSpace table.ms-listviewtable tr.ms-itmHoverEnabled div.ms-list-TitleLink>a.ms-listlink").attr("onclick", "");

		}
		
		$(document).ready(function() {
			Init();
		});
		
		
