// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your Javascript code.
var $keywordsInput = $('#search-location');
var $submitOnConfirm = $('#search-location').data('submit-on-selection');
var $defaultValue = $('#search-location').data('default-value');
if ($keywordsInput.length > 0) {
    $keywordsInput.wrap('<div id="autocomplete-container" class="das-autocomplete-wrap"></div>');
    var container = document.querySelector('#autocomplete-container');
    var apiUrl = '/locations';
    $(container).empty();
    function getSuggestions(query, updateResults) {
        var results = [];
        $.ajax({
            url: apiUrl,
            type: "get",
            dataType: 'json',
            data: { searchTerm: query }
        }).done(function (data) {
            results = data.locations.map(function (r) {
                return r.name;
            });
            updateResults(results);
        });
    }
    function onConfirm() {
        var form = this.element.parentElement.parentElement;
        setTimeout(function () {
          if (form.tagName.toLocaleLowerCase() === 'form' && $submitOnConfirm) {
                form.submit()
            }
        }, 200, form);
    }
    accessibleAutocomplete({
        element: container,
        id: 'search-location',
        name: 'Location',
        displayMenu: 'overlay',
        showNoOptionsFound: false,
        minLength: 2,
        source: getSuggestions,
        placeholder: "",
        onConfirm: onConfirm,
        defaultValue: $defaultValue,
        confirmOnBlur: false,
    });
}