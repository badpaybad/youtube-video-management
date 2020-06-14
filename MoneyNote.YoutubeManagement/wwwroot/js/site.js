var Auth = {
    login: function (uidDomId, pwdDomId) {
        var uid = jQuery('#' + uidDomId).val();
        var pwd = jQuery('#' + pwdDomId).val();

        jQuery.ajax({
            method: "POST",
            url: "/Login",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ Username: uid, Password: pwd })
        })
            .done(function (msg) {
                if (msg.code == 1) {

                    if (localStorage) { localStorage.setItem("usertoken", msg.data); }

                    window.location = "/";
                } else {
                    alert(msg.message);
                }
            });
    }
}