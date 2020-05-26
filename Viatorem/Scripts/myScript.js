
                /*switch start and end location*/
                $("#switch").click(function(){
                    var temp = $("#startLoc").val();
                    $("#startLoc").val($("#endLoc").val());
                    $("#endLoc").val(temp);
                });

                /*Bad parameters restriction*/
                    $("#submitSchedule").click(function () {
                    /*Bad start location*/
                    if ($("#startLoc").val() == null) {
                        var message = alert("You didn't select a start location");
                        return false;
                    }
                    /*Bad end location*/
                    else if ($("#endLoc").val() == null) {
                        var message = alert("You didn't select an end location");
                        return false;
                    }
                    /*Same start and end location*/
                    else if ($("#endLoc").val() == $("#startLoc").val()) {
                        var message = alert("Start and end locations cannot be same");
                        return false;
                    }

                    /*Bad date*/
                    else if ($("#date").val() == "") {
                        var message = alert("You didn't select a date");
                        return false;
                    }
                });
                                      
                /*Marking seats for reservation*/
                var obSeats=0;
               
                $("#allSeatsOne>div>i").click(function(){
                    if(obSeats<5 && $(this).hasClass("notReserved")){
                    $(this).removeClass("notReserved");
                    $(this).addClass("reserved");
                    $(this).css("color", "silver");
                    obSeats++;
                    }  
                    else if($(this).hasClass("reserved")){
                        $(this).removeClass("reserved");
                        $(this).addClass("notReserved");
                        $(this).css("color", "black");
                        obSeats--;
                    }    
                    else if ($(this).hasClass("permanentlyReserved")) {
                        var poruka = alert("This seat is already reserved"); 
                    }
                    else if(obSeats=5){
                        var poruka = alert("Not allowed reserving more than 5 seats");                       
                    }
                });

                /*Permanently reserved seats marking*/
                $("body").click(function () {
                    $(".permanentlyReserved").css("color", "red");
                });

                /*Bus Admin Seat Number Restriction*/
                $("#busFormAdmin #submit").click(function () {

                    if ($("#busFormAdmin #bus").val() < 12) {
                        var message = alert("Minimum number of seats must be 12");
                        return false;
                    }
                    else if ($("#busFormAdmin #bus").val() > 68) {
                        var message = alert("Maximum number of seats is 68");
                        return false;
                    }
                    else if ($("#busFormAdmin #bus").val() % 4 != 0) {
                        var message = alert("Number of seats must be divisible by 4");
                        return false;
                    }
                });
                /*Route Submit Restriction*/
                $("#submitRouteAdmin").click(function () {
                    if ($("#routeStartLocAdmin").val() == $("#routeEndLocAdmin").val()) {
                        var message = alert("Start and end locations cannot be same");
                        return false;
                    }
                });

                /*Reservation*/

                   $("#reservationBtn").click(function () {
                    var canReserve = true;
                    $('.time-start').each(function () {
                        var todayDate = new Date();
                        var pickedDate = new Date($(this).text());
                        if (pickedDate < todayDate) {
                            window.alert("Schedule has expired!");
                            canReserve = false;
                            return false;
                        }
                        
                       });
                       if (canReserve) {
                           var reservedId = [];

                           $('.reserved').each(function () {

                               reservedId.push($(this).attr('id'));
                           });
                           $.ajax({
                               traditional: true,
                               method: "post",
                               url: "Home/Reservate",
                               data: { 'reservedId': reservedId },
                               success: function (result) {
                                   alert(result);
                               },
                               dataType: 'text'
                           });
                       }
                    });

/*Admin rights*/
$('input[type="checkbox"]').each(function () {
    $(this).change(function () {
        var idOfUser = $(this).attr('id');
        var isChecked = "";
        if ($(this).is(':checked')) {
            isChecked = "true";
        }
        else {
            isChecked = "false";
        }
        console.log(isChecked);
        console.log(idOfUser);
        $.ajax({
            method: "get",
            url: "ChangeAdmin",
            data: { 'idOfUser': idOfUser },
            success: function (result) {
                alert(result);
            },
            dataType: 'text'
        });
    });
    });
    
                   
            
                        

                           

                /*Night mode/day mode*/

                $("#nightMode").click(function () {
                    $('body').css('background-image', 'url("https://ak0.picdn.net/shutterstock/videos/31238440/thumb/1.jpg")')
                    localStorage.setItem('bckImg', 'https://ak0.picdn.net/shutterstock/videos/31238440/thumb/1.jpg')
                });

                $("#dayMode").click(function () {
                    $('body').css('background-image', 'url("https://halftonepro.com/images/rednoise.png")')
                    localStorage.setItem('bckImg', 'https://halftonepro.com/images/rednoise.png')
                });

                $(window).on('load', function () {
                    if (localStorage.getItem('bckImg') == null) {
                        localStorage.setItem('bckImg', 'https://halftonepro.com/images/rednoise.png');
                    }
                    var n = localStorage.getItem('bckImg')
                    $('body').css('background-image', 'url('+ n +')')
                    console.log(n); 
                });

