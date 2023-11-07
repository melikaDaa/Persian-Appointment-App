
var routeURL = location.protocol + "//" + location.host;
$(document).ready(function () {

    $('#date1').MdPersianDateTimePicker({
        targetTextSelector: '#inputDate1-text',
        targetDateSelector: '#inputDate1-date',
        modal: true,

    });
  
  

    InitializeCalendar();
});
var calendar;
function InitializeCalendar() {
    var initialLocaleCode = 'fa';
    var calendarEl = document.getElementById('calendar');
    if (calendarEl != null) {
         calendar = new FullCalendar.Calendar(calendarEl, {
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
            },
            locale: initialLocaleCode,
            buttonIcons: true, // show the prev/next text
            navLinks: true, // can click day/week names to navigate views     
            selectable: true,
            editable: false,
            select: function (event) {
                onShowModal(event, null);
            },
            // allow "more" link when too many events
            eventDisplay: 'block',
            
            events: function (fetchInfo, successCallback, failureCallback) {
                $.ajax({
                    url: routeURL + '/api/Appointment/GetCalendarData?doctorId=' + $("#doctorId").val(),
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (response) {
                        var events = [];
                        if (response.status === 1) {
                            $.each(response.dataenum, function (i, data) {
                                var startDate = convertShamsiToGregorian(data.startDate);
                                var endDate = convertShamsiToGregorian(data.endDate);
                                events.push({
                                    title: data.title,
                                    desctiption: data.desctiption,
                                    start: startDate,
                                    end: endDate,
                                    backgroundColor: data.isDoctorApproved ? "#28a745" : "#dc3545",
                                    borderColor: "#162466",
                                    textColor: "white",
                                    id: data.id
                                });
                            })
                        }
                        successCallback(events);
                    },
                    error: function (xhr) {
                        $.notify("Error", "error");
                    }
                });
            },
            eventClick: function (info) {
                getEventDetailsByEventId(info.event);
            }
        });
        calendar.render();
    }
}

// در این تابع تاریخ شمسی به تاریخ میلادی تبدیل می‌شود.
function convertShamsiToGregorian(shamsiDate) {
    var gregorianDate = moment(shamsiDate, "YYYY/MM/DD");
    return gregorianDate.toDate();
}


//نمایش مدال 
function onShowModal(obj, isEventDetails) {
    if (isEventDetails != null) {
        fillReservationStatusFields(obj);
    } else {
        fillNewReservationFields(obj);
    }
    $("#appointmentInput").modal("show");
}

//// این تابع برای پر کردن فیلدهای فرم مشخص شده استفاده می‌شود.
function fillReservationStatusFields(obj) {
    $("#title, #description, #inputDate1-text, #duration, #doctorId, #patientId, #id").val(function () {
        return obj[this.id];
    });

    $("#lblPatientName").html(obj.patientName);
    $("#lblDoctorName").html(obj.doctorName);
    $("#lblStatus").html(obj.isDoctorApproved ? 'رزرو شده' : 'در انتظار رزرو');

    if (obj.isDoctorApproved) {
        $("#btnConfirm, #btnSubmit").addClass("d-none");
    } else {
        $("#btnConfirm, #btnSubmit").removeClass("d-none");
    }

    $("#btnDelete").removeClass("d-none");
}
// این تابع اطلاعات رزرو جدید را پر می‌کند.
function fillNewReservationFields(obj) {
    $("#inputDate1-text").val(obj.startStr + " " + moment().format("hh:mm A"));
    $("#id").val(0);
    $("#btnDelete").addClass("d-none");
    $("#btnSubmit").removeClass("d-none");
}
//این تابع مدال میبنده و تمام فیلد مقدارشون پاک میکنه
function onCloseModal() {
    var fieldsToReset = ["#title", "#description", "#inputDate1-date", "#duration", "#patientId", "#id"];
    
    $(fieldsToReset.join(", ")).val('');
    
    $("#appointmentForm")[0].reset();
    $("#appointmentInput").modal("hide");
}
// تابع onSubmitForm برای ارسال فرم به سرور استفاده می‌شود.
function onSubmitForm() {
  
    var requestData = getFormData();
    sendRequest(requestData);
}
// تابع getFormData برای جمع‌آوری اطلاعات از فرم استفاده می‌شود.
function getFormData() {
    return {
        Id: parseInt($("#id").val()),
        Title: $("#title").val(),
        Description: $("#description").val(),
        StartDate: $("#inputDate1-date").val(),
        Duration: $("#duration").val(),
        DoctorId: $("#doctorId").val(),
        PatientId: $("#patientId").val(),
    };
}
// تابع sendRequest برای ارسال درخواست به سرور با استفاده از AJAX استفاده می‌شود.
function sendRequest(requestData) {
    $.ajax({
        url: routeURL + '/api/Appointment/SaveCalendarData',
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: handleResponse,
        error: handleRequestError
    });
}
// تابع handleResponse برای پردازش پاسخ موفقیت‌آمیز درخواست استفاده می‌شود.
function handleResponse(response) {
    if (response.status === 1 || response.status === 2) {
        calendar.refetchEvents();
        $.notify(response.message, "success");
        onCloseModal();
    } else {
        $.notify(response.message, "error");
    }
}
// تابع handleRequestError برای پردازش خطاهای درخواست استفاده می‌شود.
function handleRequestError(xhr) {
    $.notify("Error", "error");
}

// تابع getEventDetailsByEventId برای درخواست جزئیات رویداد با شناسه مشخص استفاده می‌شود.
function getEventDetailsByEventId(info) {
    fetchDataByEventId(info.id, onSuccess, onError);
}
// تابع fetchDataByEventId برای انجام درخواست به سرور برای دریافت جزئیات رویداد استفاده می‌شود.
function fetchDataByEventId(eventId, successCallback, errorCallback) {
    $.ajax({
        url: `${routeURL}/api/Appointment/GetCalendarDataById/${eventId}`,
        type: 'GET',
        dataType: 'JSON',
        success: onSuccess,
        error: onError
    });
}
// تابع onSuccess برای پردازش پاسخ موفقیت‌آمیز درخواست استفاده می‌شود.
function onSuccess(response) {

    if (response.status === 1 && response.dataenum !== undefined) {
        onShowModal(response.dataenum, true);
    }
    successCallback(events);
}
// تابع onError برای پردازش خطاهای درخواست استفاده می‌شود.
function onError (xhr) {
    $.notify("Error", "error");
}
// تابع onDoctorChanges برای تغییرات در انتخاب دکتر توسط کاربر استفاده می‌شود.
function onDoctorChanges() {
    calendar.refetchEvents();
}
// تابع onDeleteAppointment برای حذف یک رزرو تعیین شده توسط کاربر به کار می‌رود.
function onDeleteAppointment() {
    var id = parseInt($("#id").val());
    deleteAppointmentById(id, onDeleteSuccess, onDeleteError);
}
// تابع deleteAppointmentById برای ارسال درخواست حذف به سرور استفاده می‌شود.
function deleteAppointmentById(appointmentId, successCallback, errorCallback) {
    $.ajax({
        url: `${routeURL}/api/Appointment/DeleteAppointment/${appointmentId}`,
        type: 'GET',
        dataType: 'JSON',
        success: successCallback,
        error: errorCallback
    });
}
// تابع onDeleteSuccess برای پردازش نتیجه موفقیت‌آمیز حذف وقوعه به کار می‌رود.
function onDeleteSuccess(response) {

    if (response.status === 1) {
        $.notify(response.message, "success");
        calendar.refetchEvents();
        onCloseModal();
    }
    else {
        $.notify(response.message, "success");
    }
}
// تابع onDeleteError برای پردازش نتیجه خطا‌آمیز حذف وقوعه به کار می‌رود.
function onDeleteError (xhr) {
    $.notify("Error", "error");
}
// تابع onConfirm برای تأیید ‌رزرو تعیین شده توسط کاربر به کار می‌رود.
function onConfirm() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeURL + '/api/Appointment/ConfirmEvent/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: onConfirmSuccess,
        error: onConfirmError
    });
}
// تابع onConfirmSuccess برای پردازش نتیجه موفقیت‌آمیز تأیید وقوعه به کار می‌رود.
function onConfirmSuccess(response) {

    if (response.status === 1) {
        $.notify(response.message, "success");
        calendar.refetchEvents();
        onCloseModal();
    }
    else {
        $.notify(response.message, "success");
    }
}
// تابع onConfirmError برای پردازش خطاهای مرتبط با درخواست تأیید وقوعه به کار می‌رود.
function onConfirmError (xhr) {
    $.notify("Error", "error");
}