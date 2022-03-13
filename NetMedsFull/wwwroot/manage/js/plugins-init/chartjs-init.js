(function($) {
  "use strict" 

	
	/* function draw() {
		
	} */

 var dlabSparkLine = function(){
	let draw = Chart.controllers.line.__super__.draw; //draw shadow
	
	var screenWidth = $(window).width();
	
	var barChart1 = function(){
		if(jQuery('#barChart_1').length > 0 ){
			const barChart_1 = document.getElementById("barChart_1").getContext('2d');
    
			barChart_1.height = 100;

			new Chart(barChart_1, {
				type: 'bar',
				data: {
					defaultFontFamily: 'Poppins',
					labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul"],
					datasets: [
						{
							label: "My First dataset",
							data: [65, 59, 80, 81, 56, 55, 40],
							borderColor: 'rgba(136,108,192, 1)',
							borderWidth: "0",
							backgroundColor: 'rgba(136,108,192, 1)'
						}
					]
				},
				options: {
					legend: false, 
					scales: {
						yAxes: [{
							ticks: {
								beginAtZero: true
							}
						}],
						xAxes: [{
							barPercentage: 0.5
						}]
					}
				}
			});
		}
	}
	



	/* Function ============ */
	return {
		init:function(){
		},
		
		
		load:function(){
			barChart1();	
			barChart2();
			barChart3();	
			lineChart1();	
			lineChart2();		
			lineChart3();
			lineChart03();
			areaChart1();
			areaChart2();
			areaChart3();
			radarChart();
			pieChart();
			doughnutChart(); 
			polarChart(); 
		},
		
		resize:function(){
			// barChart1();	
			// barChart2();
			// barChart3();	
			// lineChart1();	
			// lineChart2();		
			// lineChart3();
			// lineChart03();
			// areaChart1();
			// areaChart2();
			// areaChart3();
			// radarChart();
			// pieChart();
			// doughnutChart(); 
			// polarChart(); 
		}
	}

}();

jQuery(document).ready(function(){
});
	
jQuery(window).on('load',function(){
	dlabSparkLine.load();
});

jQuery(window).on('resize',function(){
	//dlabSparkLine.resize();
	setTimeout(function(){ dlabSparkLine.resize(); }, 1000);
});
	
})(jQuery);