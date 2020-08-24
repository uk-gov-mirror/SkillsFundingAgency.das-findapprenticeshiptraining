// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
var $keywordsInput = $('#search-location');

if ($keywordsInput.length > 0) {

    var container, apiUrl;

    $keywordsInput.wrap('<div id="fat-autocomplete-container"></div>');
    container = document.querySelector('#fat-autocomplete-container');
        apiUrl = '/locations';

    $(container).empty();
    var getSuggestions = function (query, updateResults) {

        var results = [];
        $.ajax({
            url: apiUrl,
            type: "get",
            dataType: 'json',
            data: { searchTerm: query }

        }).done(function (data) {
            results = data.locations.map(function (r) {
                return r.locationName;
            });
            
            updateResults(results);
        });
    };

    accessibleAutocomplete({
        element: container,
        id: 'Locations',
        name: 'Locations',
        displayMenu: 'overlay',
        showNoOptionsFound: false,
        minLength: 3,
        source: getSuggestions,
        placeholder: ""
    });
}
