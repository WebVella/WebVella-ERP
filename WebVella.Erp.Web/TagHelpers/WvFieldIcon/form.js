function IconFieldFormGenerateSelectors(fieldId) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.inputControl = "#input-" + fieldId;
	return selectors;
}

function IconFieldFormInit(fieldId) {
	var selectors = IconFieldFormGenerateSelectors(fieldId);

	$(selectors.inputControl).select2({
		ajax: {
			url: '/api/v3.0/p/core/select/font-awesome-icons',
			data: function (params) {
				var query = {
					search: params.term,
					page: params.page || 1
				};
				return query;
			},
			dataType: 'json',
			processResults: function (data) {
				// Tranforms the top-level key of the response object from 'items' to 'results'
				var results = [];
				if(data.object.results){
					_.forEach(data.object.results,function(rec){
						results.push({id:rec.class,text:rec.class,name:rec.name});
					});
				}

				data.object.results = results;
				return data.object;
			}
		},
		//language: "bg",
		placeholder: 'not-selected',
		allowClear: !$(selectors.inputControl).prop('required'),
		closeOnSelect: true,
		width: 'element',
		escapeMarkup: function (markup) {
			return markup;
		},
		templateResult: function (state) {
			var $state = $(
				'<div class="erp-ta-icon-result"><div class="icon-wrapper"><i class="icon fa-fw ' + state.id + '"/></div><div class="meta"><div class="title">' + state.id + '</div><div class="entity go-gray">' + state.name + '</div></div>'
			);
			return $state;
		}
	});

	$(selectors.inputControl).on('select2:select', function (e) {
		
		$(selectors.inputControl).closest(".input-group").find(".input-group-prepend .fa-fw").attr("class","fa-fw " + e.target.value);
	});

	$(selectors.inputControl).on("select2:unselecting", function (e) {
		$(this).data('state', 'unselected');
	}).on("select2:open", function (e) {
		if ($(this).data('state') === 'unselected') {
			$(this).removeData('state');
			var self = $(this);
			setTimeout(function () {
				self.select2('close');
			}, 1);
		}
	});

}

