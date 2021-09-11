// اعادة حجوزات العميل 
function AjaxGetCustomerBooking(DivName, customerguid) {
    //debugger
    $.ajax({
        method: 'post',
        url: '../DailyBookings/GetBookings',
        data: { customerguid: customerguid},
        success: function (data) {
            $("#formSearchBookings").hide();
            $("#" + DivName + "").show();
            $("#" + DivName + "").empty();
            //debugger
            $("#" + DivName + "").html(data);
        }
    });
}

// اعادة حجوزات قسم معين لتاريخ معين
function AjaxGetBookingsFromDate(DivName) {
    
    $.ajax({
        method: 'GET',
        url: '../DailyBookings/Create',
        data: { date: $("#input_Date").val(), sectionGuid: $("#Section_Guid :selected").val() },
        success: function (data) {
            $("#" + DivName + "").empty();
            $("#DIV_DailyBookings").hide();
            //debugger
            $("#" + DivName + "").html(data);
        }
    });
}

// اضافة الحجز
function AjaxCreateCustomerBooking(DivName, customerguid, bookingGuid) {
    //debugger
    $.ajax({
        method: 'POST',
        url: '../DailyBookings/Create',
        data: { customerguid: customerguid, bookingGuid: bookingGuid },
        success: function (data) {
            $("#" + DivName + "").empty();
            $("#formSearchBookings").hide();
            //debugger
            //$("#" + DivName + "").html(data);

            AjaxGetCustomerBooking(DivName, customerguid);
        }
    });
}

// الغاء الحجز
function AjaxCancelCustomerBooking(DivName, customerguid, bookingGuid) {
    //debugger
    $.ajax({
        method: 'POST',
        url: '../DailyBookings/Edit',
        data: { customerguid: customerguid, bookingGuid: bookingGuid },
        success: function (data) {
            $("#" + DivName + "").empty();
            $("#formSearchBookings").hide();
            //debugger
            //$("#" + DivName + "").html(data);

            AjaxGetCustomerBooking(DivName, customerguid);
        }
    });
}
