// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
var $keywordsInput = $('#search-location');

if ($keywordsInput.length > 0) {

    $keywordsInput.wrap('<div id="autocomplete-container" class="das-autocomplete-wrap"></div>');
    var container = document.querySelector('#autocomplete-container');
    var apiUrl = '/locations';

    $(container).empty();

    function getSuggestions (query, updateResults) {
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
    }

    function onConfirm() {
        var form = this.element.parentElement.parentElement;  
        setTimeout(function(){
          if (form.tagName.toLocaleLowerCase() === 'form') {
            form.submit()
      }
    },200,form);}

    accessibleAutocomplete({
        element: container,
        id: 'Locations',
        name: 'Locations',
        displayMenu: 'overlay',
        showNoOptionsFound: false,
        minLength: 3,
        source: getSuggestions,
        placeholder: "",
        onConfirm: onConfirm
    });
}
