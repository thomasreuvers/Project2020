﻿function Translate() {
    //initialization
    this.init = function (attribute, lng) {

        var lang = null;

        if (LanguageExists("js/resources/" + lng + ".json")) {
            lang = lng;
        } else {
            lang = "en";
        }

        this.attribute = attribute;
        _self = this;
        var xrhFile = new XMLHttpRequest();
        var url = `js/resources/${lang}.json`;

        //load content data 
        xrhFile.open("GET", url, false);

        xrhFile.onreadystatechange = function() {
            if (xrhFile.readyState === 4) {
                if (xrhFile.status === 200 || xrhFile.status == 0) {
                    var LngObject = JSON.parse(xrhFile.responseText);
                    console.log(LngObject["name1"]);
                    var allDom = document.getElementsByTagName("*");
                    for (var i = 0; i < allDom.length; i++) {
                        var elem = allDom[i];
                        var key = elem.getAttribute(_self.attribute);

                        if (key != null) {
                            console.log(key);
                            elem.innerHTML = LngObject[key];
                        }
                    }

                }
            }
        }
        xrhFile.send();
    }
}

function LanguageExists(url) {
    $.ajax(url,
            {
                method: 'GET',
                dataType: 'jsonp'
            })
        .done(function (response) {
            console.log("exists");
            return true;
        }).fail(function (response) {
            console.log("doesn't exist");
            return false;
        });
}


$(document).ready(function() {
    var translate = new Translate();
    var currentLng = navigator.language.substring(0, 2) || navigator.userLanguage.substring(0, 2);
    var attributeName = 'data-tag';
    translate.init(attributeName, currentLng);
});


// Toggle showing sidebar
$(".toggle__SideBar").on("click", function() {

    $(".sidebar").toggleClass("hide-bar");
    console.log("hello");
});



// Add new table row
var i = $("#table-body tr").length - 2;

$(".new-exercise").on("click",
    function () {
        i++;
        $("#table-body").after($(".bg-dark"))
            .append($("<tr>")
                .append($("<td>").append($(`<input class="form-control" type="text" id="Exercises_${i}__Name" name="Exercises[${i}].Name" value="">`)))
                .append($("<td>").append($(`<input class="form-control" type="text" id="Exercises_${i}__Description" name="Exercises[${i}].Description" value="">`)))
                .append($("<td>").append($(`<input class="form-control" type="text" id="Exercises_${i}__Sets" name="Exercises[${i}].Sets" value="">`)))
                .append($("<td>").append($(`<input class="form-control" type="text" id="Exercises_${i}__Reps" name="Exercises[${i}].Reps" value="">`)))
                .append($("<td>").append($(`<input class="form-control" type="text" id="Exercises_${i}__Weight" name="Exercises[${i}].Weight" value="">`)))
            );
    });

// Remove last table row
$(".remove-exercise").click(function () {
    if (i > 0) {
        i--;
        $("#table-body > tr:last").prev().remove();
    }
});