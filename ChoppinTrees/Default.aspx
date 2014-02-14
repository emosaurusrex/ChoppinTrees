<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ChoppinTrees.Default" %>
<%@ Register Assembly="Framework" Namespace="Framework.Controls" TagPrefix="trees" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
	<title>Choppin' Trees - Mug'Thol</title>
	<link href="http://fonts.googleapis.com/css?family=Oswald|Open+Sans" rel="stylesheet" type="text/css">
	<link href="http://code.jquery.com/ui/1.10.4/themes/hot-sneaks/jquery-ui.css" rel="stylesheet" type="text/css">
	<link href="/style.css" rel="stylesheet" type="text/css">
    <link href="/classes.css" rel="stylesheet" type="text/css">
	<script src="jquery.min.js"></script>
	<script src="/calendar/fullcalendar.min.js"></script>
	<script src="/calendar/gcal.js"></script>
	<link rel="stylesheet" href="calendar/fullcalendar.css" />

	<script>
	    $(function () {
	        $('#calendar').fullCalendar({
	            events: 'https://www.google.com/calendar/feeds/mr.jiggles%40gmail.com/public/basic',
	            theme: true,
	            header: {
	                left: 'month,basicWeek',
	                center: 'title',
	                right: 'prev,next'
	            }
	        });
	    });
	</script>
</head>
<body>
    <trees:LeftPane runat="server" />
	<!--<div id="leftPane">
		<div id="titlePane">
			<h1>choppinTREES</h1>
			<p class="content">We are CHOPPIN' TREES, a Mug'thol - US Horde guild. Our main roster is made up of a group of friends from the Quad City area with a long WoW history getting back into raiding. We mix a 6 hour raid week with a mature atmosphere, allowing us to maintain real lives but still see above-casual progression. Sound good? Apply <a href="#">here</a>.</p>
		</div>
		<div id="externalLinks">
			<h1>external Links</h1>
			<ul class="content">
				<li><a href="#" target="_blank">Raid logs</a></li>
				<li><a href="#" target="_blank">Armory</a></li>
				<li><a href="#" target="_blank">I'm sure there's more</a></li>
			</ul>
		</div>
		<div id="events" class="leftPane">
			<h1>events</h1>
			<div id="calendar"></div>
		</div>
	</div>!-->
	<div id="nav">
		<div class="container">
			<div class="l-triangle-top"></div>
			<div class="l-triangle-bottom"></div>
			<div class="rectangle">
				<ul>
					<li><a href="#" class="active">Home</a></li>
					<li><a href="#">Apply</a></li>
					<li><a href="#">Loot History/DKP</a></li>
				</ul>
			</div>
			<div class="r-triangle-top"></div>
			<div class="r-triangle-bottom"></div>
		</div>
	</div>
	<div id="content" runat="server">
	</div>
</body>
</html>