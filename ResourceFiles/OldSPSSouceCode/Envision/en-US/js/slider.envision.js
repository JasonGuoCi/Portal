ExecuteOrDelayUntilScriptLoaded(function(){
 
    //Take a copy of the existing Microsoft Definition of RenderListView
    var oldRenderListView = RenderListView;
 
    //Now redefine RenderListView with our override
    RenderListView = function(ctx,webPartID)
    {
        //Check the context of the currently rendering List view
        
        var title = $("#WebPartTitle"+webPartID+" h2 span:first").text();
        if(!IsStrNullOrEmpty(title) && title == "EnvisionAnnouncements")
        {
	        		HideChromeTitle(webPartID);
					ctx.BaseViewID = 100;
        }
        //now call the original RenderListView
        oldRenderListView(ctx,webPartID);
    }
 
},"ClientTemplates.js");
function HideChromeTitle(webPartID)
{
	var inDesignMode = document.forms[MSOWebPartPageFormName].MSOLayout_InDesignMode.value;
	if(IsStrNullOrEmpty(inDesignMode))
	{
		document.getElementById("WebPart"+webPartID+"_ChromeTitle").style.display="none";
	}		
}
/*
IT News Carousel -- Start *************************************************************
*/

var ITNewsCarousel = ITNewsCarousel || {};
ITNewsCarousel.RegisterTemplateOverrides = function()
{
	var overrideCtx = {};
	overrideCtx.Templates = {};
	
//	Assign functions or plain html strings to the templateset objects:
//	header, footer and item.
	
	overrideCtx.Templates.Header = ITNewsCarousel.HeaderHTML;
	overrideCtx.Templates.Item = ITNewsCarousel.ItemHTML;
	overrideCtx.Templates.Footer = ITNewsCarousel.FooterHTML;

	//overrideCtx.BaseViewID = 1;
	overrideCtx.ListTemplateType = 104;
		
//  Register the template overrides.
	console.log(overrideCtx);
	SPClientTemplates.TemplateManager.RegisterTemplateOverrides(overrideCtx);
}
ITNewsCarousel.HeaderHTML = function(ctx)
{
	var a = [];
	a.push('<div class="slides_outerContainer" id="slides_outerContainer_'+ctx.wpq+'">'); 
	a.push('    <div class="slides_innerContainer" id="slides_innerContainer_'+ctx.wpq+'">');
	a.push('        <div class="slides_container" id="slides_container_'+ctx.wpq+'">');
	a.push('            <div class="slides_control" webPartId="'+ctx.wpq+'">');
	a.push('            	<div class="slide-row">');
	return a.join("");
}
ITNewsCarousel.ItemHTML= function(ctx)
{	
	console.log(ctx.CurrentItem);
	var ID = ctx.CurrentItem["ID"];
	var bgColor = ctx.CurrentItem["BackgroundColor"];
	var listName = ctx.listName;
	var exUrl = ctx.CurrentItem["URL"];
	var exShowTitle = ctx.CurrentItem["Show_x0020_Title"];
	var exTitleColor = ctx.CurrentItem["Title_x0020_Font_x0020_Color"];
	var exUrlText = ctx.CurrentItem["URL.desc"];
	var exDescFont = ctx.CurrentItem["Description_x0020_Font_x0020_Style"];
	var exUrlSize = ctx.CurrentItem["URL_x0020_Font_x0020_Size"];
	//var exUrlColor = ctx.CurrentItem["URL_x0020_Font_x0020_Color"];
	var exCTAColor = ctx.CurrentItem["CTA_x0020_Color"];
	var exTextColor = ctx.CurrentItem["Text_x0020_Color"];
	

	if (exTextColor == "Dark") {
	    isDark = "ctaDark";
	    exUrlColor = "#000";
	    exTitleColor = "#000";
	    titleColor = "#000";
    }
	else {
	    isDark = "";
	    exUrlColor = "#fff";
	    exTitleColor = "#fff";
	    titleColor = "#fff";
    }


	if (exShowTitle == "Yes") {
	    showTitle = true;
	} else {
	    showTitle = false;
	}


	if (exDescFont == "Light") {
	    exDescFontFamily = "Segoe UI Light";
	} else {
	    exDescFontFamily = "Segoe UI";
	}
	if (exUrlText == exUrl) {
	    exUrlText = "Read More";
	}



    /*
	if (exShowTitle == "Yes") {
		showTitle = true;
	} else {
		showTitle = false;
	}
	if (exTitleColor == "Black") {
		titleColor = "#000";
	} else {
		titleColor = "#fff";
	}
	if (exDescFont == "Light") {
		exDescFontFamily = "Segoe UI Light";
	} else {
		exDescFontFamily = "Segoe UI";
	}
	if (exUrlText == exUrl) {
		exUrlText = "Read More";
	}
	if (exCTAColor == "Black") {
		exCTAFileName = "readmore.png";
		exUrlColor = "#fff";
	} else {
		exCTAFileName = "readmore_dark.png";
		exUrlColor = "#000";
	}
*/


	var targetUrl = _spPageContextInfo.webServerRelativeUrl+"/pages/detail.aspx?listName="+listName+"&listItem="+ID;
	

	var title = ctx.CurrentItem["Title"];
	var a = [];
	a.push('<div class="slide-cell">');
	a.push('<div class="slide_container social_config_scope">');
	a.push('			<div class="slide-image">');
	a.push('				<div class="slide_imageContainer">');
								//	a.push('<a href="'+targetUrl+'" title="'+title+'">');
									var pictureUrl = ctx.CurrentItem["PublishingRollupImage"];
									if(isDefinedAndNotNullOrEmpty(pictureUrl))
									{		            
								        var regx = /(?:src=)["|']([^?"']*)/gi;
								        regx.test(pictureUrl);
								        var imgSrc = RegExp.$1;
								        a.push('<img alt="' + title + '" src="' + imgSrc + '?width=512&height=318"/>');        
									}
									else
									{
										a.push('<img alt="' + title + '" src="' + _spPageContextInfo.webServerRelativeUrl+'/SiteAssets/Default_318.png?width=512&height=318"/>');
									}
								//	a.push('</a>');	
	a.push('            	</div>');
	a.push('			</div>');
	a.push('			<a href="'+exUrl+'">');
	a.push('			<div class="ms-EmphasisBackground-bgColor" style="background-color: '+bgColor+'">');
    a.push('    			<div class="slide_contentContainer slide_inheritBackground" >');	            
	a.push('		            <div class="slide_contentInfo">');
	if (showTitle) {
		a.push('			            <div class="nc_hotNewsTitle">');
		//a.push('				            <a href="'+targetUrl+'" title="'+title+'">');
		a.push('				                <span class="ms-Wrap" style="color: '+titleColor+';">'+title+'</span>');
		//a.push('				            </a>');
		a.push('			            </div>');
	}
	

	var newsDescription = ctx.CurrentItem["Body"];
	var maxLen = 500;
	if(!IsStrNullOrEmpty(newsDescription) && newsDescription.length > maxLen)
	{		
		newsDescription = newsDescription.substring(0,maxLen)+"...";
		a.push('			        <div class="nc-hotNewsDescription ms-Wrap nc-hide" style="font-family: '+exDescFontFamily+'; color: '+titleColor+'">' + newsDescription.substring(0,maxLen)+'...</div>');
	}	
	else
	{
	    a.push('			        <div class="nc-hotNewsDescription ms-Wrap nc-hide" style="font-family: ' + exDescFontFamily + '; color: ' + titleColor + '">' + newsDescription + '</div>')
	}
	a.push('			            <div class="nc-hotNewsInfo ms-noWrap">');
	
	var newsPublisher = ctx.CurrentItem["_Author"];
	if(isDefinedAndNotNullOrEmpty(newsPublisher))
	{
	    a.push('    					<span>By '+newsPublisher+'</span>');     	
	}  	
	
	var newsPublishDate = STSHtmlDecode(ctx.CurrentItem["Publish_x0020_Date"]);
	if(isDefinedAndNotNullOrEmpty(newsPublishDate))
	{	
		var dates = new Date(newsPublishDate).format("dddd MMMM dd"); 	              	
	    a.push('						<span>'+dates+'</span>');    
	}		                	
	a.push('			            </div>');
	a.push('		            </div>');
	
	a.push('					<div class="divSocialToolbarHost divSocialToolbar ms-textSmall ms-noWrap" id="divSocialToolbarHost_'+ID+'">');
    a.push('						<ul class="ulSocialToolbarH '+isDark+'">');
//	a.push('							<li class="liLike" id="spLike_'+ID+'"><div class="SocialLike"></div></li>');
    a.push('							<li class="liShare">');
    a.push('								<div class="social_config_custom" unselectable="on" data-object="{Email:\'True\',Facebook:\'True\',Twitter:\'False\',LinkedIn:\'False\',Yammer:\'True\',ShareTarget:\'.nc_hotNewsTitle a\'}"></div>');
//	a.push('									<a title="Share" class="SocialToolbar" href="javascript:" id="share_'+ID+'">');
//	a.push('        								<a href="'+exUrl+'" title="'+title+'">');
//	a.push('										<span class="sprites Share_ICON"></span>');
	a.push('										<span class="spShareLabel" style="font-size: '+exUrlSize+'px; color: '+exUrlColor+';">'+exUrlText+'</span > ');
//	a.push('									</a>');							
	a.push('							</li>');
	a.push('						</ul>');
	a.push('					</div>');
	a.push('				</div>');
    a.push('    		</div>');
    a.push('    		</a>');
    a.push('    		<div class="ms-tableCell">');
    a.push('    			<div class="slide_separator ms-tableCell"></div>');
    a.push('    		</div>');        			        			
        	      
    a.push('</div>');
    a.push('</div>');
    
	return a.join("");
}

ITNewsCarousel.FooterHTML= function(ctx)
{
	var a = [];
	a.push('<div class="slides_pagingBar" id="slides_pagingBar_'+ctx.wpq + '">');    
    a.push('    <ul class="slides_pagination">');    
    for(var i = 0; i < ctx.ListData.Row.length; i++){
    	if(i==0)
		{		
			a.push('	<li class="current">');
		}
		else
		{
			a.push('	<li>');
		}
	    a.push('    	<a href="javascript:{}" onclick="ITNewsCarousel.Slides_pagingClicked('+i+',\''+ctx.wpq+'\');">');
	    a.push('        	<span>●</span>');
	    a.push('        </a>');
	    a.push('    </li>');
    }
    a.push('	</ul>');
    a.push('</div>');
	$('#slides_outerContainer_'+ctx.wpq).append(a.join(""));
    ITNewsCarousel.Slides_init(ctx.wpq);
	return ""; 
}
ITNewsCarousel.Slides_init = function(webPartId){
	var controlDiv = document.querySelectorAll('#slides_container_'+webPartId)[0];
	
	//var slideWidth = $(controlDiv).width()+1;
	//var imageWidth = $(controlDiv).find(".slide_imageContainer").width();
	//$(controlDiv).find(".slide_contentContainer").css("max-width",slideWidth-imageWidth);
	//$(controlDiv).find(".slide_container").css("max-width",slideWidth);
	//$(controlDiv).attr("slideWidth",slideWidth);
	
	if(!controlDiv.timer){
		var timer = document.createAttribute("timer");
        controlDiv.setAttributeNode(timer);
        var firstSlide = $(controlDiv).find(".slides_control .slide-row>div:first-child").clone();
        var lastSlide = $(controlDiv).find(".slides_control .slide-row>div:last-child").clone();
		$(controlDiv).find(".slides_control .slide-row").append(firstSlide);
		$(controlDiv).find(".slides_control .slide-row").prepend(lastSlide);
		$(controlDiv).mouseover(function()
		{
			clearTimeout(controlDiv.timer);
		});
		$(controlDiv).mouseout(function()
		{
			var current = $("#slides_pagingBar_"+ webPartId +" li.current").index();
			ITNewsCarousel.Slides_timer(webPartId, current);
		});
		var slideContainerDiv = document.querySelectorAll('#slides_container_'+webPartId + ' DIV.slide_container')[0];
		var slideWidth = slideContainerDiv.clientWidth;

		$(controlDiv).find(".slides_control").animate({top:"-100%"},0,ITNewsCarousel.Slides_callback)
											//.animate({left:-1*805},0,ITNewsCarousel.Slides_callback);
											.animate({left:-1*slideWidth},0,ITNewsCarousel.Slides_callback);
        ITNewsCarousel.Slides_timer(webPartId, 0);
    }
    
	// THIS IS A WEIRD HACK BUT WHATEVER
	var pageWidth = $(window).width();
	if (pageWidth < 1150 && pageWidth > 830) {		
		var offset = 1138 - pageWidth + 15;
		$('.slide_container div.ms-EmphasisBackground-bgColor').css('position', 'relative').css('right', offset);
	} else {
		$('.slide_container div.ms-EmphasisBackground-bgColor').css('position', 'relative').css('right', '0');
	}
   
};
ITNewsCarousel.Slides_timer = function(webPartId, currentIdx){
	var controlDiv = document.querySelectorAll('#slides_container_'+webPartId)[0];
	
    var TimerDelayMilliSeconds = 5000;    
    currentIdx ++;
    var numResults = $("#slides_pagingBar_"+ webPartId +" li").length;
    if(currentIdx >= numResults){
        currentIdx = 0;
    }
    else if(currentIdx < 0)
    {
        currentIdx = numResults - 1;
    }
    clearTimeout(controlDiv.timer);
    controlDiv.timer = setTimeout(function(){ITNewsCarousel.Slides_changeSlide(currentIdx, webPartId); controlDiv = null; currentIdx = null;}, TimerDelayMilliSeconds);
};
ITNewsCarousel.Slides_pagingClicked = function(clicked, webPartId)
{
	var controlDiv = document.querySelectorAll('#slides_container_'+webPartId)[0];	
    var current = $("#slides_pagingBar_"+ webPartId +" li.current").index();
    var next = parseInt(clicked,10);
    if(current !== next )
    {	    	
		clearTimeout(controlDiv.timer);
		ITNewsCarousel.Slides_changeSlide(clicked, webPartId);
	}
}
ITNewsCarousel.Slides_changeSlide = function(clicked, webPartId){
	var current = $("#slides_pagingBar_"+ webPartId +" li.current").index();	
	
	var controlDiv = document.querySelectorAll('#slides_container_'+webPartId)[0];
    var next = parseInt(clicked,10);
   // var slideWidth = 805;
   var slideContainerDiv = document.querySelectorAll('#slides_container_'+webPartId + ' DIV.slide_container')[0];
    var slideWidth = slideContainerDiv.clientWidth;
    
    if(current !== next )
    {	    	
    	var x = -(next+1)*slideWidth;
    	var y = -(next+1)*100 + "%";
    	var spd = Math.abs(next-current)*1000;
    	if($("#slides_pagingBar_"+ webPartId +" li:last-child").index() == current && 
    	$("#slides_pagingBar_"+ webPartId +" li:first-child").index() == next)
    	{
    		spd = 1000;
    	}
    	$("#slides_pagingBar_"+ webPartId +" li.current").removeClass("current")
		$("#slides_pagingBar_"+ webPartId +" li").eq(next).addClass("current");
		
		if($(window).width() < 768)
		{
			$(controlDiv).find(".slides_control").animate({top:y},spd,ITNewsCarousel.Slides_callback)
								.animate({left:x},0);
		}
		else
		{
			$(controlDiv).find(".slides_control").animate({left:x},spd,ITNewsCarousel.Slides_callback)
								.animate({top:y},0);
		}	
		
    	$(controlDiv).find(".slides_control").animate({left:x},spd,ITNewsCarousel.Slides_callback);
    	ITNewsCarousel.Slides_timer(webPartId, next);
    	if($("#slides_pagingBar_"+ webPartId +" li:last-child").index() == next){    		
    		if($(window).width() < 768)
			{
				$(controlDiv).find(".slides_control").animate({top:0},0,ITNewsCarousel.Slides_callback);
			}
			else
			{
				$(controlDiv).find(".slides_control").animate({left:0},0,ITNewsCarousel.Slides_callback);
			}
    	}
    }
    // THIS IS A WEIRD HACK BUT WHATEVER
	var pageWidth = $(window).width();
	if (pageWidth < 1150 && pageWidth > 830) {		
		var offset = 1138 - pageWidth + 15;
		$('.slide_container div.ms-EmphasisBackground-bgColor').css('position', 'relative').css('right', offset);
	} else {
		$('.slide_container div.ms-EmphasisBackground-bgColor').css('position', 'relative').css('right', '0');
	}
    
};
ITNewsCarousel.Slides_callback = function()
{
	var webPartId = $(this).attr("webPartId");
	var idx = $("#slides_pagingBar_"+ webPartId +" li.current").index();
	idx++;
	if(idx == $("#slides_pagingBar_"+ webPartId +" li").length)
	{
		idx = 0;
	}
	$('#slides_container_'+webPartId+' .slides_control').attr("class","slides_control slide-" + idx);
}
/*
IT News List TwoColumn -- Start *************************************************************
*/

var ITNewsListViewsTwoColumn = ITNewsListViewsTwoColumn || {};
ITNewsListViewsTwoColumn.RegisterTemplateOverrides= function()
{
	var overrideCtx = {};
	overrideCtx.Templates = {};
	
//	Assign functions or plain html strings to the templateset objects:
//	header, footer and item.
	
	overrideCtx.Templates.Header = ITNewsListViewsTwoColumn.HeaderHTML;
	overrideCtx.Templates.Item = ITNewsListViewsTwoColumn.ItemHTML;
	overrideCtx.Templates.Footer = ITNewsListViews.FooterHTML;

	overrideCtx.BaseViewID = 101;
	overrideCtx.ListTemplateType = 100;
		
//   	 Register the template overrides.

	SPClientTemplates.TemplateManager.RegisterTemplateOverrides(overrideCtx);
};
ITNewsListViewsTwoColumn.HeaderHTML = function (ctx){
	var a = [];
	a.push('<div style="float: right;">'); 
	RenderPaging(a,ctx);
	a.push('</div>');	
	a.push('<div class="recentNews-container">');	
	a.push(RenderTableHeader(ctx).replace("<table ", "<table style='width: 100%; padding: 0px; margin: 0px;' "));
	return a.join("");
};
ITNewsListViewsTwoColumn.ItemHTML = function (ctx){
	var iStr = [];
    if(ctx.CurrentItemIdx%2 == 0)
    {
	    iStr.push('<tr>');
    }
    iStr.push('<td style="width: 50%; vertical-align: top;">');
    iStr.push(ITNewsListViews.ItemHTML(ctx));
    iStr.push('</td>');    
    if(ctx.CurrentItemIdx%2 == 1)
    {
	    iStr.push('</tr>');
    }
    else if(ctx.CurrentItemIdx+1 == ctx.ListData.Row.length)
    {
    	iStr.push('<td></td></tr>');
    }
    return iStr.join('');
};
ITNewsListViewsTwoColumn.FooterHTML = function(ctx){
	return "";
};

/*
IT News List -- Start ***************************************************
*/
var ITNewsListViews = ITNewsListViews || {};
ITNewsListViews.RegisterTemplateOverrides= function()
{
	var overrideCtx = {};
	overrideCtx.Templates = {};
	
//	Assign functions or plain html strings to the templateset objects:
//	header, footer and item.
	
	overrideCtx.Templates.Header = ITNewsListViews.HeaderHTML;
	overrideCtx.Templates.Item = ITNewsListViews.ItemHTML;
	overrideCtx.Templates.Footer = ITNewsListViews.FooterHTML;

	overrideCtx.BaseViewID = 100;
	overrideCtx.ListTemplateType = 100;
		
//   	 Register the template overrides.

	SPClientTemplates.TemplateManager.RegisterTemplateOverrides(overrideCtx);
};
ITNewsListViews.HeaderHTML = function (ctx){
	var a = [];
	a.push('<div style="float: right;">'); 
	RenderPaging(a,ctx);
	a.push('</div>');	
	a.push('<div class="recentNews-container">');	
	return a.join("");
};
ITNewsListViews.ItemHTML = function(ctx){

	var a = [];
	//a.push('');
	a.push('<div class="recentNews-item-container social_config_scope">');
	
	var ID = ctx.CurrentItem["ID"];
	var listName = ctx.listName;
	var exUrl = ctx.CurrentItem["URL"];
	var targetUrl = _spPageContextInfo.webServerRelativeUrl+"/pages/detail.aspx?listName="+listName+"&listItem="+ID;
	var title = ctx.CurrentItem["Title"];
		
	var pictureUrl = ctx.CurrentItem["PublishingRollupImage"];
	if(isDefinedAndNotNullOrEmpty(pictureUrl))
	{
		a.push('<div class="recentNews-item-imageContainer">');
     //   a.push('<a href="' + targetUrl +'" title="'+title+'" >');              
      var regx = /(?:src=)["|']([^?"']*)/gi;
        regx.test(pictureUrl);
        var imgSrc = RegExp.$1;
        a.push('<img alt="' + title + '" src="' + imgSrc + '?width=142&height=84"/>');
       // a.push('</a>');
        a.push('</div>');
        a.push('<div class="recentNews-item-contentContainer">');
	}	
	else
	{
		a.push('<div class="recentNews-item-contentContainerNoPicture">'); 
	}          	
    a.push('   <div class="rencentNews-title">');
	//a.push('        <a href="'+targetUrl+'" title="'+title+'">');
	a.push('            <span class="ms-Wrap">'+title+'</span>');
//	a.push('        </a>');
    a.push('   </div>');
       
    var newsPublisher = ctx.CurrentItem["_Author"];
	if(isDefinedAndNotNullOrEmpty(newsPublisher))
	{
		a.push('   <div class="rencentNews-info ms-noWrap">');
	    a.push('    	<span>By '+newsPublisher+'</span>');     	
	    a.push('   </div>');
	}        
	                     
    var newsPublishDate = STSHtmlDecode(ctx.CurrentItem["Publish_x0020_Date"]);
	if(isDefinedAndNotNullOrEmpty(newsPublishDate))
	{	
		var dates = new Date(newsPublishDate).format("dddd MMMM dd"); 	
		a.push('   <div class="rencentNews-date">');                	
	    a.push('		<span>'+dates+'</span>');    
	    a.push('   </div>');
	}
    
    a.push('<div class="rencentNews-toolbar">');
    var newsSource =ctx.CurrentItem["NewsSource"];
	if(isDefinedAndNotNullOrEmpty(newsSource)&&isDefinedAndNotNullOrEmpty(newsSource.Label))
	{
		a.push('<div class="branding"><span>'+newsSource.Label+'</span></div>');
	}
	a.push('<div class="divSocialToolbarHost divSocialToolbar ms-textSmall ms-noWrap" id="div1">');
    a.push('<ul class="ulSocialToolbarH">');
    //a.push('<li class="liLike" id="Li1"><div class="SocialLike"></div></li>');
    	a.push('<span class="social_config_custom" unselectable="on" data-object=\'{Email:"True",Facebook:"True",Twitter:"False",LinkedIn:"False",Yammer:"True",ShareTarget:".rencentNews-title a"}\'></sp>');
   
	 a.push('<li class="liShare">');
	//a.push('	<a title="Share" class="SocialToolbar" href="javascript:" id="share_'+ID+'">');
	a.push('        <a href="'+exUrl+'" title="'+title+'">');
//	a.push('	<span class="sprites Share_ICON"></span>');
	a.push('	<span class="spShareLabel">Read More</span > ');
	a.push('	</a>');							
	a.push('</li>');
	
	a.push('</ul>');
	a.push('</div>');
	a.push('</div>');
	a.push('</div>');
	a.push('</div>');
	return a.join("");

};

ITNewsListViews.FooterHTML = function(ctx){
	return "";
};

RegisterModuleInit("/Style Library/Envision/en-US/js/slider.envision.js", RegisterViewTemplates); 
RegisterViewTemplates(); 

function RegisterViewTemplates() {
    //ITNewsListViews.RegisterTemplateOverrides();
    //ITNewsListViewsTwoColumn.RegisterTemplateOverrides();
    ITNewsCarousel.RegisterTemplateOverrides();
};


(function () {
    //ITNewsListViews.RegisterTemplateOverrides();
    //ITNewsListViewsTwoColumn.RegisterTemplateOverrides();
    ITNewsCarousel.RegisterTemplateOverrides();
})();
 

