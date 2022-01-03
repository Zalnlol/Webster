$(function () {
//This line initialize when checkbox AcceptUserAgreement is click
    $("#UserRegistrationModal input[name='AcceptUserAgreement']").click(onAcceptUserAgreementClick);

    //This line set 'Register' button disabled by default
    $("#UserRegistrationModal button[name = 'register']").prop("disabled", true);

    //This function will let 'Register' button clickable when checkbox is ticked
    function onAcceptUserAgreementClick()
    {
        if ($(this).is(":checked")) {
            $("#UserRegistrationModal button[name = 'register']").prop("disabled", false);
        }
        else {
            $("#UserRegistrationModal button[name = 'register']").prop("disabled", true);
        }
    }
    //-------------------------------------------------------------------------------------------------
    //Set input text for email to be onBlur (lose focus) and display error message "Email already register"
    $("#UserRegistrationModal input[name='Email']").blur(function () {
        var email = $("#UserRegistrationModal input[name='Email']").val();
        var url = "UserAuth/UserNameExists?userName=" + email;
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                if (data == true) {   
                    PresentClosableBootstrapAlert("#alert_placeholder_register", "warning", "Invalid Email", "This email address has already been registered");
                }
                else {
                    //$("#alert_placeholder_register").html("");

                    CloseAlert();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;
                PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        });
    });

    //-------------------------------------------------------------------------------------------------
    var registerUserButton = $("#UserRegistrationModal button[name='register']").click(onUserRegisterClick);

    function onUserRegisterClick() {
        var url = "UserAuth/RegisterUser";
        var antiForgeryToken = $("#UserRegistrationModal input[name='__RequestVerificationToken']").val();
        var email = $("#UserRegistrationModal input[name='Email']").val();
        var password = $("#UserRegistrationModal input[name='Password']").val();
        var confirmPassword = $("#UserRegistrationModal input[name='ConfirmPassword']").val();
        var firstName = $("#UserRegistrationModal input[name='FirstName']").val();
        var lastName = $("#UserRegistrationModal input[name='LastName']").val();
        var phoneNumber = $("#UserRegistrationModal input[name='PhoneNumber']").val();

        var user = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            FirstName: firstName,
            LastName: lastName,
            PhoneNumber: phoneNumber,
            AcceptUserAgreement: true,
        };

        $.ajax({
            type: "POST",
            url: url,
            data: user,
            success: function (data)
            {
                var parsed = $.parseHTML(data);
                var hasError = $(parsed).find("input[name = 'RegistrationInValid']") == 'true';

                //Display registration modal box to user again in case there was failed attempt
                if (hasError == true) {
                    $("#UserRegistrationModal").html(data);
                    //Wiring to function register click event to register button
                    registerUserButton = $("#UserRegistrationModal button[name='register']").onUserRegisterClick();

                    var form = $("#UserRegistrationForm");
                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                else
                {
                    location.href = 'Home/Index';
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            {
                var errorText = "Status:" + xhr.status + "-" + xhr.statusText;
                PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        });

    }
});
