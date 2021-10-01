// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    //SetAccountName(10, "");
    //SetEmail(10, "", "@kdg.be");
    //$("#Active").prop("checked", false);
    //$("#AccountType").change();
    AutoCompleteGroup();
	SetActive();
	SetRGDetails()

});

//$("#AccountType").focusout(function () {
//    GenerateAccountName();
//	GenerateEmail();
//	SetRightsGroup();
//	SetActive();
//});

$("#AccountType").change(function () {
	GenerateAccountName();
	GenerateEmail();
	SetRightsGroup();
	SetActive();
});

$("#RightsGroup").change(function () {
	SetRGDetails();
});

$("#Domain").focusout(function () {
    GenerateAccountName();
    GenerateEmail();
});

$("#FirstName").focusout(function () {
    GenerateAccountName();
    GenerateDisplayName();
    GenerateEmail();
});

$("#LastName").focusout(function () {
    GenerateAccountName();
    GenerateDisplayName();
    GenerateEmail();
});

$("#AccountName").focusout(function () {
    var AccountName = $("#AccountName");
    console.log(AccountName);
});

function AutoCompleteGroup() {

    $("#GroupAccountName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Users/GetADGroups",
                type: "POST",
                dataType: "json",
                data: { term: request.term, domain: $("#Domain").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item, value: item };
                    }))
                }
            })
        },
        messages: {
            noResults: '',
            results: function () {''}
        }
    });
}


function GenerateAccountName() {
    var FirstName = $("#FirstName").val().trim();
    var LastName = $("#LastName").val().trim();
    var AccountType = $("#AccountType").val();
    var account = "";
    var path = window.location.pathname;

    if (path.includes("/Edit/")) {
        return;
    }

    if (LastName != null && LastName != '') {
        LastName = LastName.toLowerCase();
        LastName = LastName.replace(/de |den |der |le |op |opde |van |vande|vander| |gi/, "");
        LastName = LastName.replace(" ", "");
        LastName = LastName.substr(0, 7);
        account = LastName;
    }

    if (FirstName != null && FirstName != '') {
        FirstName = FirstName.toLowerCase();
        FirstName = FirstName.substr(0, 1);
        account = account + FirstName;
    }

    if (AccountType === undefined) { AccountType = 10}

    SetAccountName(AccountType, account);
}

function GenerateDisplayName() {
    var FirstName = $("#FirstName").val();
    var LastName = $("#LastName").val();
    var DisplayName = $('#DisplayName');

    if (FirstName != null && FirstName != '' && LastName != null && LastName != '') {
        DisplayName.val(LastName + ' ' + FirstName);
    } else if (FirstName != null && FirstName != '' && (LastName == null || LastName == '')) {
        DisplayName.val(FirstName);
    } else if (FirstName != null || FirstName != '' && (LastName != null && LastName != '')) {
        DisplayName.val(LastName);
    } else { }
}

function GenerateEmail() {
    var AccountType = $("#AccountType").val();
    var Domain = $("#Domain").val()
    var atdomain = '@kdg.be';
    var path = window.location.pathname;
    if (path.includes("/Edit/")) {
        return;
    }
    if (Domain == 'STUDENT') {
        atdomain = '@student.kdg.be';
    }

    if (AccountType === undefined) { AccountType = 10 }

    SetEmail(AccountType, GenerateMailNickName(), atdomain);

}

function GenerateMailNickName() {
    var FirstName = $("#FirstName").val().trim();
    var LastName = $("#LastName").val().trim();
    var mailnickname = "";

    if (FirstName != null && FirstName != '') {
        FirstName = FirstName.toLowerCase();
        FirstName = FirstName.replace(" ", "");
        mailnickname = FirstName + '.';
    } else {
        mailnickname = '';
    }

    if (LastName != null && LastName != '') {
        LastName = LastName.toLowerCase();
        LastName = LastName.replace(" ", "");
        mailnickname = mailnickname + LastName;
    }
    return mailnickname;
}

function SetAccountName(selectedAccountType, account) {
    var AccountName = $("#AccountName");
    $.getJSON('/Users/GetAccountExt', { Id: selectedAccountType }, function (results) {
        if (results != null && !jQuery.isEmptyObject(results)) {
            AccountName.val(account + results.accountExt);
        }
    });
}

function SetActive() {
    var AccountType = $("#AccountType").val();
    var Active = $("#Active");
	var ActiveLbl = $("#ActiveLbl"); console.log(ActiveLbl);
	if (window.location.href.includes("/Edit/")) {
		return;
	}
    if (AccountType === undefined) { AccountType = 10 }
    if (document.getElementById("ActiveLbl") !== null) {
        Active.prop("checked", true);
        if (AccountType == 10) {
            Active.prop("checked", false);
            document.getElementById("ActiveLbl").innerText = "Shared mailbox = uitvinken!";
        }
        else {
            document.getElementById("ActiveLbl").innerText = "Actief";
        }
    }
}

function SetEmail(selectedAccountType, mailnickname, atdomain) {
    var Email = $("#Email");
    $.getJSON('/Users/GetAccountExt', { Id: selectedAccountType }, function (results) {
        if (results != null && !jQuery.isEmptyObject(results)) {
            Email.val(mailnickname + results.emailExt + atdomain);
        }
    });
}

function SetRightsGroup() {
	var AccountType = $("#AccountType").val();
	var $RightsGroup = $('#RightsGroup');
	$.getJSON('/RightsGroups/GetDDLGroups', { accounttype: AccountType }, function (results) {
		if (results != null && !jQuery.isEmptyObject(results)) {
			$RightsGroup.empty();
			for (var i = 0; i < results.length; i++) {
				$RightsGroup.append('<option value=' + results[i].id + '>' + results[i].name + '</option>');
			}
			$RightsGroup.change();
		}
	});
}

function SetRGDetails() {
	var RGId = $('#RightsGroup').val();
	var $RGDetails = $('#RGDetails');
	$.getJSON('/RightsGroups/GetRGDetails', { id: RGId }, function (results) {
		if (results != null && !jQuery.isEmptyObject(results)) {
			var details = '<h4>Rechtengroep details</h4>';
			details += '<h5>Naam: ' + results[0].Name + '</h5>';
			details += '<ul>';
			details += '<li> Max. eind datum: ' + results[0].MaxEndDate + ' (weken)</li>';
			details += '<li>AD Groepen</li>'
			if (results[0].Kdggroup.length > 0) {
				details += '<ul>';
				for (i = 0; i < results[0].Kdggroup.length; i++) {
					details += '<li>' + results[0].Kdggroup[i].Domain + '\\' + results[0].Kdggroup[i].AccountName + '</li>';
				}
				details += '</ul>';
			}
			details += '<li>Properties</li>'
			if (results[0].Kdgrightsgrouppropertie.length > 0) {
				details += '<ul>';
				for (i = 0; i < results[0].Kdgrightsgrouppropertie.length; i++) {
					details += '<li>' + results[0].Kdgrightsgrouppropertie[i].Field + ' (' + results[0].Kdgrightsgrouppropertie[i].Type + '): ' + results[0].Kdgrightsgrouppropertie[i].Value + '</li>';
				}
				details += '</ul>';
			}
			details += '</ul>';
			$RGDetails.html(details);
		}
	});
}


