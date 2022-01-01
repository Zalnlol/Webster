$(function () {

    var registerUserButton = $("#UserRegistrationModal button[name='register']").click(onUserRegisterClick);

    function onUserRegisterClick() {
        var url = "/UserAuth/RegisterUser";
        var antiForgeryToken = $("#UserRegistrationModal input[name='__RequestVerificationToken']").val();
        var email = $("#UserRegistrationModal input[name='Email']").val();
        var password = $("#UserRegistrationModal input[name='Password']").val();
        var confirmPassword = $("#UserRegistrationModal input[name='ConfirmPassword']").val();
        var firstName = $("#UserRegistrationModal input[name='FirstName']").val();
        var lastName = $("#UserRegistrationModal input[name='LastName']").val();
        var phoneNumber = $("#UserRegistrationModal input[name='PhoneNumber']").val();

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            FirstName: firstName,
            LastName: lastName,
            PhoneNumber: phoneNumber,
            AcceptUserAgreement: true
        };

        $.ajax({
            type: "POST",
            url: url,
            data: userInput,
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
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        });

    }
});