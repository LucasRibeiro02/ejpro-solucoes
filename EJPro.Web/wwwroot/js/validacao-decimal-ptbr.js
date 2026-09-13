(function () {
    if (!window.jQuery || !jQuery.validator) {
        return;
    }

    function converterNumeroBrasileiro(valor) {
        return Number(
            valor
                .replace(/\./g, "")
                .replace(",", ".")
        );
    }

    jQuery.validator.methods.number = function (value, element) {
        if (this.optional(element)) {
            return true;
        }

        return /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    };

    jQuery.validator.methods.range = function (value, element, parametros) {
        if (this.optional(element)) {
            return true;
        }

        const numero = converterNumeroBrasileiro(value);

        return numero >= parametros[0] &&
            numero <= parametros[1];
    };
})();