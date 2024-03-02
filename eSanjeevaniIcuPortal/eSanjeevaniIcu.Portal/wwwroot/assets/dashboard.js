var _totalCardCount = 12;

$(document).ready(function () {
    $("#textDate").val((new Date()).toISOString().split('T')[0]);
    GetPatientDetails(function (resData) {
        $("#divPatientCard").empty();
        buildRebuildPartientDetailCard(resData);
    });
});

function buildRebuildPartientDetailCard(resData) {
    let htmlPatientCard = "";
    for (var i = 1; i <= _totalCardCount; i++) {
        var objPatientDetails = resData[i - 1];
        htmlPatientCard += '<div class="col-md-3">';
        if (objPatientDetails != undefined) {
            htmlPatientCard += '<div class="card text-center" style="cursor: pointer;" onclick="onclickPatientCard(this)">';
            htmlPatientCard += '<input type="hidden" id="hdnPatientDetailId" value="' + objPatientDetails.patientId + '"/>';
        }
        else {
            htmlPatientCard += '<div class="card text-center" style="cursor: pointer;">';
        }
        htmlPatientCard += '<div style="padding:5px">'
        if (objPatientDetails != undefined) {
            if (objPatientDetails.patientStatusName == 'Normal') {
                htmlPatientCard += '<div class="card-body normal-bg">';
            }
            else if (objPatientDetails.patientStatusName == 'High') {
                htmlPatientCard += '<div class="card-body high-bg">';
            }
            else if (objPatientDetails.patientStatusName == 'Critical') {
                htmlPatientCard += '<div class="card-body critical-bg">';
            }
           // htmlPatientCard += '<div class="col-md-4">';
           // htmlPatientCard += '<label>Bed ' + objPatientDetails.bedNo + '</label>';
           // htmlPatientCard += '</div>';
           // htmlPatientCard += '<div class="col-md-4">';
           // htmlPatientCard += '<label class="dashboardCardPatientName">' + objPatientDetails.patientName + '</label>';
           // htmlPatientCard += '</div>';
           // htmlPatientCard += '<div class="col-md-4">';
           //// htmlPatientCard += '<label>' + objPatientDetails.patientId + '</label> <button class="btn btn-primary pull-right"><i class="fa fa-phone"></i>Call</button>';
           // htmlPatientCard += '<button class="btn btn-primary pull-right">' + objPatientDetails.patientCode + ' <i class="fa fa-phone"></i></button>';
           // htmlPatientCard += '</div>';
           // htmlPatientCard += '</div>';
            htmlPatientCard += '<div class="row">';
            htmlPatientCard += '<div class="col-md-5">';
            htmlPatientCard += '<label class="dashboardCardPatientName">' + objPatientDetails.patientName + '</label>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '<div class="col-md-3">';
            htmlPatientCard += '<label class="dashboardCardPatientName">' + objPatientDetails.genderName + '</label>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '<div class="col-md-4">';
            htmlPatientCard += '<label class="dashboardCardPatientName">Age :' + objPatientDetails.age + '</label>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row default-bg">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-solid fa-temperature-half"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan">' + objPatientDetails.temperature + '</div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row default-bg">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-tint"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan">' + objPatientDetails.oxygenSaturationSpo2 + '(%)</div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row default-bg">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-heartbeat"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan">' + objPatientDetails.heartRate + '</div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row default-bg">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-solid fa-lungs"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan">' + objPatientDetails.rrRespiratoryRate + '</div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row default-bg">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class=" fa fa-solid fa-wave-square"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan">' + objPatientDetails.bloodPressureSys + '/' + objPatientDetails.bloodPressureDia + '</div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';
        }
        else {
            htmlPatientCard += '<div class="row">';
            htmlPatientCard += '<div class="col-md-4 blank-bg">';
            htmlPatientCard += '<label>null</label>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '<div class="col-md-4">';
            htmlPatientCard += '<label></label>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '<div class="col-md-4">';
            htmlPatientCard += '<label></label>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-solid fa-temperature-half"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan"> - </div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row pt-2">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-tint"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan"> - (%)</div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-heartbeat"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan"> - </div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class="fa fa-solid fa-lungs"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan"> - </div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';

            htmlPatientCard += '<div class="row">';
            htmlPatientCard += '<div class="col-md-12 top-less-border">';
            htmlPatientCard += '<div class="col-md-4 dashboardCardSpan"><i class=" fa fa-solid fa-wave-square"></i></div>';
            htmlPatientCard += '<div class="col-md-8 dashboardCardSpan"> - </div>';
            htmlPatientCard += '</div>';
            htmlPatientCard += '</div>';
        }
        htmlPatientCard += '</div>';
        htmlPatientCard += '</div>';
        htmlPatientCard += '</div>';
        htmlPatientCard += '</div>';
    }
    $("#divPatientCard").append(htmlPatientCard);
}

function GetPatientDetails(callback) {
    var url = apiUrl + "/DashBoard/GetPatientDetails/";
    $.ajax({
        url: url,
        data: { HubHospitalId: ($("#ddlHubHospital").val() == "" ? null : $("#ddlHubHospital").val()), SpokeHospitalId: ($("#ddlSpokeHospital").val() == "" ? null : $("#ddlSpokeHospital").val()), date: ($("#textDate").val() == "" ? null : $("#textDate").val()) },
        cache: false,
        type: "GET",
        //beforeSend: function () {
        //    $("#progressModal").attr("style", "display:block;background:rgb(0 0 0 / 38%);");
        //},
        success: function (response) {
            if (response.success) {
                var dayBookRows = response.data;
                callback(response.data);
            } else {
                //swal("Error", response.responseText, "error");
            }
        },
        error: function (error) {
        }
        ,
        complete: function () {
            //$("#progressModal").hide()
        }
    });
}

$("#ddlHubHospital").change(function () {
    $("#ddlSpokeHospital").empty();
    var url = apiUrl + "/DashBoard/GetSpokeHospital/";
    $.ajax({
        url: url,
        data: { HubHospitalId: $(this).val() },
        cache: false,
        type: "GET",
        //beforeSend: function () {
        //    $("#progressModal").attr("style", "display:block;background:rgb(0 0 0 / 38%);");
        //},
        success: function (response) {
            let optionSpokeHospital = "<option value=''>Select Spoke Hopital</option>";
            if (response.length > 0) {
                $(response).each(function (i, op) {
                    optionSpokeHospital += "<option value='" + op.value + "'>" + op.text + "</option>";
                })
            }
            $("#ddlSpokeHospital").append(optionSpokeHospital);
        },
        error: function (error) {
        }
        ,
        complete: function () {
            //$("#progressModal").hide()
        }
    });
})

function filterPatientDetails() {
    GetPatientDetails(function (resData) {
        $("#divPatientCard").empty();
        buildRebuildPartientDetailCard(resData);
    });
}

function onclickPatientCard(eve) {
     var patientDetailId = $(eve).find('#hdnPatientDetailId').val();
    if (patientDetailId != "" && patientDetailId != 0)
        window.location = apiUrl + "/DashBoard/Details/" + patientDetailId;
}