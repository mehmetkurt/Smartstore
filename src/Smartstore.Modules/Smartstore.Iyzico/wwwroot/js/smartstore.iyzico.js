"use strict";

/*
 *  Project: smartstore.iyzico
 *  Version: 1.0
 *  Author: Mehmet KURT
 */
; (function ($) {

    $.fn.extend({
        iyzico: function (options) {
            var _previousCardNumber = "";
            var defaults = {
                installmentCallbackUrl: "",
                installmentTable: "",
                container: null,
                formInputs: {
                    cardNumber: 'input#CardNumber',
                    expirationDate: 'input#ExpirationDate',
                    cardCode: 'input#CardCode',
                    cardholderName: 'input#CardholderName',
                    creditCardType: 'input#CreditCardType'
                },
                formEl: {
                    cardNumber: null,
                    expirationDate: null,
                    cardCode: null,
                    cardholderName: null
                },
                messages: {
                    creditCardNotValid: "Credit card not valid!",
                    installmentsLoading: "When installment options are loading.<br />Please wait.It will only take a moment!"
                }
            };

            options = $.extend(true, {}, defaults, options);

            // set form elements from form inputs
            options.formEl.cardNumber = $(options.formInputs.cardNumber);
            options.formEl.expirationDate = $(options.formInputs.expirationDate);
            options.formEl.cardCode = $(options.formInputs.cardCode);
            options.formEl.cardholderName = $(options.formInputs.cardholderName);
            options.formEl.creditCardType = $(options.formInputs.creditCardType);

            options.container = $(this);

            const _init = function () {
                _prepareMaskedElements();
                options.formEl.cardNumber.on("blur", function () {
                    const unmaskedValue = $(this).inputmask('unmaskedvalue');
                    _getInstallments(unmaskedValue);
                    $(this).val(unmaskedValue);
                });
            }
            const _prepareMaskedElements = function () {
                let _self = this;
                $(options.formEl.expirationDate).inputmask({
                    alias: "datetime",
                    inputFormat: "dd/yy",
                    placeholder: "dd/yy",
                });

                $(options.formEl.cardCode).inputmask({
                    mask: "[9][9][9][9]",
                    placeholder: "____",
                    greedy: true,
                    definitions: {
                        '9': {
                            validator: "[0-9]",
                            cardinality: 1,
                        }
                    },
                    autoUnmask: true,
                });

                $(options.formEl.cardNumber).inputmask({
                    mask: "9999-9999-9999-9999",
                    placeholder: " ",
                    greedy: true,
                    definitions: {
                        '9': {
                            validator: "[0-9]",
                            cardinality: 1
                        }
                    },
                    autoUnmask: true,
                    onBeforeMask: function (value, opts) {
                        return value.replace(/-/g, '');
                    },
                    onBeforePaste: function (pastedValue, opts) {
                        var cleanedValue = pastedValue.replace(/[-_ ]/g, '');
                        if (!/^\d+$/.test(cleanedValue)) {
                            return "";
                        }

                        return cleanedValue;
                    },
                    onUnMask: function (maskedValue, unmaskedValue) {
                        return unmaskedValue;
                    }
                });
            };

            const _getInstallments = function (cardNumber) {
                //const cardNumber = options.formEl.cardNumber.val();
                if (cardNumber.length < 6)
                    return;

                if (cardNumber === _previousCardNumber)
                    return;

                if (_validataCreditCardNumber()) {
                    var postData = {
                        BinNumber: cardNumber
                    };

                    _hideAndClearInstallmentContent();

                    $.throbber.show({
                        message: options.messages.installmentsLoading
                    });

                    $.ajax({
                        type: 'POST',
                        url: options.installmentCallbackUrl,
                        data: postData,
                        success: function (response) {
                            if (response.success) {
                                _prepareInstallmentTable(response.data.InstallmentPrices);
                                const creditCardType = `${response.data.CardAssociation}|${response.data.CardFamilyName}`;
                                options.formEl.creditCardType.val(creditCardType);
                            }
                            else {
                                displayNotification(response.message, response.messageType);
                            }
                        },
                        complete: function () {
                            $.throbber.hide();
                        }
                    });
                }
                else {
                    displayNotification(options.messages.creditCardNotValid, 'error');
                }

                _previousCardNumber = cardNumber;
            };

            const _hideAndClearInstallmentContent = function () {
                const installmentTable = options.container.find(options.installmentTable);
                const tableBody = installmentTable.find("tbody");
                tableBody.html("");
                installmentTable.addClass("d-none");

                return installmentTable;
            }
            const _prepareInstallmentTable = function (installmentPrices) {
                const installmentTable = _hideAndClearInstallmentContent();
                const tableBody = installmentTable.find("tbody");

                installmentPrices.forEach(installment => {
                    const row = $("<tr></tr>");
                    const cellInstallmentRadio = $("<td scope='row'></td>");
                    const cellInstallmentPrice = $("<td></td>");
                    const cellTotalPrice = $("<td></td>");
                    const cellInstallmentNumber = $("<td></td>");

                    const radio = document.createElement('input');
                    radio.type = 'radio';
                    radio.name = 'Installment';
                    radio.value = installment.InstallmentNumber;

                    cellInstallmentRadio.append(radio);
                    cellInstallmentPrice.text(installment.InstallmentPrice);
                    cellTotalPrice.text(installment.TotalPrice);
                    cellInstallmentNumber.text(installment.InstallmentNumber);

                    row.append(cellInstallmentRadio);
                    row.append(cellInstallmentNumber);
                    row.append(cellInstallmentPrice);
                    row.append(cellTotalPrice);

                    tableBody.append(row);
                });

                if (installmentTable.length > 0 && installmentTable.is('table')) {

                    let firstRadioInput = installmentTable.find('input[type="radio"]').first();
                    if (firstRadioInput.length > 0) {
                        firstRadioInput.prop('checked', true);
                    }

                    installmentTable.on('click', 'tr', function () {
                        let radioInput = $(this).find('input[type="radio"]');
                        if (radioInput.length > 0) {
                            radioInput.prop('checked', true);
                        }
                    });
                }

                installmentTable.removeClass("d-none");
            };

            const _validataCreditCardNumber = function () {
                const cardNumber = options.formEl.cardNumber.val();

                var checksum = 0;
                var j = 1;
                for (var i = cardNumber.length - 1; i >= 0; i--) {
                    var calc = 0;
                    calc = Number(cardNumber.charAt(i)) * j;
                    if (calc > 9) {
                        checksum = checksum + 1;
                        calc = calc - 10;
                    }
                    checksum = checksum + calc;
                    if (j == 1) {
                        j = 2;
                    } else {
                        j = 1;
                    }
                }

                var result = (checksum % 10) == 0;
                return result;
            }

            _init();

            return options.container;
        }
    })
})(jQuery, window, document);
