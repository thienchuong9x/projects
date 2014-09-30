$(document).ready(function () {

    function HoverOver() {
        $(this).addClass('rmhover') .find(".subMenu").stop().slideDown(200).show();
    }

    function HoverOut() {
         $(this).removeClass('rmhover').find(".subMenu").stop().slideUp(200, function() {
		  $(this).hide(); 
	  });
    }
	
	function HoverOversub() {
        $(this).find(".subMenuRight").stop().slideDown(150).show();
    }

    function HoverOutsub() {
         $(this).find(".subMenuRight").stop().slideUp(150, function() {
		  $(this).hide(); 
	  });
    }

    var config = {
        sensitivity: 2,
        interval:  200,
        over: HoverOver,
        timeout:200,
        out: HoverOut
    };
	
	   var configsub = {
        sensitivity: 2,
        interval:  150,
        over: HoverOversub,
        timeout:150,
        out: HoverOutsub
    };

    $("#standardMenu .rootMenu > li.haschild").hoverIntent(config);
	$("#standardMenu .subMenu li.haschild").hoverIntent(configsub);

});