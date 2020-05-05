(function ($) {
    function AboutUs() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-AboutUs").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AboutUs();
        self.init();
    })
}(jQuery))