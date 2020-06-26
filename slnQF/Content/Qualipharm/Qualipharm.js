$(document).ready(

    function () {
         $(document).on('change', '.changeLoteNuevo', function () {
            var tipo = $('#lbltipo').val();

            $("#btnNuevoLote").show();
            if (tipo == "ORDEN") {
                var floatTotal = 0.0000000;
                var floatTotalFinal = 0.0000000;
                var $v_cantidadPlani = parseFloat($('#txt_cantidad').val());
                $("#tblNLotes tbody tr").each(function (index) {
                    var campo1, campo2, campo3, campo4, campo5, campo6;
                    var $v_contador = 0;
                    $(this).children("td").each(function (index2) {
                        var v_campo1;
                        var v_campo2;
                        var v_campo3;
                        var v_campo4;
                        var v_campo5;
                        var v_campo6;
                        var v_campoRenglon;

                        $(this).find("input").each(function () {
                            $v_contador = $v_contador + 1;
                            if ($v_contador == 1) {
                                v_campo1 = this.value;
                            }
                            if ($v_contador == 2) {
                                v_campo2 = this.value;
                            }
                            if ($v_contador == 3) {
                                v_campo3 = this.value;

                            }
                            if ($v_contador == 4) {
                                v_campo4 = this.value;

                            }
                            if ($v_contador == 5) {
                                v_campo5 = this.value;

                            }
                            if ($v_contador == 6) {
                                v_campo6 = this.value;

                            }

                        });

                        switch (index2) {
                            case 1: campo1 = v_campo1;
                                break;
                            case 2: campo2 = v_campo2;
                                break;
                            case 3: campo3 = v_campo3;
                                break;
                            case 4: campo4 = v_campo4;
                                break;
                            case 5: campo5 = v_campo5;
                                break;
                            case 6: campo6 = v_campo6;
                                break;
                        }
                        //$(this).css("background-color", "#ECF8E0");
                    });
                    var obj = new Object();
                    obj.ITEMCODE = 0;
                    obj.BATCHNUM = campo1;
                    obj.QUANTITY = 0;
                    obj.TTOTAL = campo2;
                    obj.FECHA_EXP = campo3;
                    obj.NUEVO = 1;
                    obj.MNFSERIAL = campo4;
                    obj.LOTNUMBER = campo5;
                    obj.LOTNOTES = campo6;
                    floatTotal = floatTotal + parseFloat(obj.TTOTAL);
                
                });
                floatTotalFinal = ($v_cantidadPlani - floatTotal).toFixed(6);
                $("#lblCantidadRestante").text("Cantidad restante: " + floatTotalFinal);
                console.log(parseFloat(floatTotalFinal).toFixed(6));
                console.log(parseFloat($v_cantidadPlani).toFixed(6));
                if (parseFloat(floatTotal).toFixed(6) != parseFloat($v_cantidadPlani).toFixed(6)) {
                    $("#btnNuevoLote").hide();

                    $("#lbl_MensajeNuevoLote").text('Corregir las cantidades, no coincide con la cantidad solicitada.');
                } else {
                    $("#btnNuevoLote").show();

                    $("#lbl_MensajeNuevoLote").text('');
                }
            
            }
        });


        $(document).on('click', '.CofigLoteClose', function () {
            var $vTabla =$("#lbl_tableSelected").text();
        
            $('#' + $vTabla).remove();
        });
   
        $(document).on('click', '.ConfigLoteNuevoClose', function () {
            $("#txtNewCantidad").val($('#txt_cantidad').val());

            $("#lbl_MensajeNuevoLote").text('');
            $("#lblCantidadRestante").text("Cantidad restante: 0.0" );
        });
    
        $(document).on('change', '.changeLote', function () {

            var tr = $(this).parents('tr');
            var $item = $(this).closest("tr");
            var $v_itemcode = $item.children("td:eq(0)").html();
            var $v_cantidadTotal ;
        
            var listOfObjects = [];
            var div2 = document.getElementById('divLotes');
            var tablas = [];
            for (i = j = 0; i < div2.childNodes.length; i++) {
                if (div2.childNodes[i].nodeName == 'TABLE') {
                    j++;
                    var input = div2.childNodes[i].id;
                    tablas.push(input);

                }
            }

        
            for (var r = 0, n = tablas.length; r < n; r++) {
                var contador = 0;
                $("#" + tablas[r] + " tbody tr").each(function (index) {
                    var campo1, campo2, campo3, campo4;


                    $(this).children("td").each(function (index2) {

                        var v_campo4;
                        var v_campoRenglon;
                        var $v_contador = 0;
                        $(this).find("input").each(function () {

                            $v_contador = $v_contador + 1;
                            if ($v_contador == 1) {
                                v_campo4 = this.value;

                            }

                        })

                        switch (index2) {
                            case 0: campo1 = $(this).text();
                                break;
                            case 1: campo2 = $(this).text();
                                break;
                            case 2: campo3 = $(this).text();
                                break;
                            case 3: campo4 = v_campo4;
                                break;
                        }


                        //$(this).css("background-color", "#ECF8E0");
                    })

                    if (contador > 0) {
                        var obj = new Object();
                        obj.ITEMCODE = campo1;
                        obj.BATCHNUM = campo2;
                        obj.QUANTITY = campo3;
                        if (campo4.length <= 0) {
                            campo4 = 0;
                        }   
                        obj.TTOTAL = campo4;
                        obj.FECHA_EXP = "";
                        obj.NUEVO = "0";
                        if (tablas[r] == "tblLotes" + $v_itemcode){
                            listOfObjects.push(obj);
                        }
                    }
                    contador = contador + 1;

                });
            }

            $v_cantidadTotal = 0.0;
            for (i = 0; i < listOfObjects.length; i++) {
                $v_cantidadTotal = parseFloat($v_cantidadTotal) + parseFloat(listOfObjects[i].TTOTAL)

                console.log(parseFloat($v_cantidadTotal));
            }
        
            var $v_CantidadSolicitada = parseFloat($("#lbl_quantity").text());

            var $diferenciaTot = parseFloat($v_CantidadSolicitada).toPrecision() - parseFloat($v_cantidadTotal).toPrecision();
        
            $("#lblCantidadRestanteConfigLote").text($diferenciaTot.toPrecision(6));
        
            if (parseFloat($diferenciaTot) != 0) {
                $("#btnConfLote").hide();

                $("#lbl_MensajeConfigLote").text('Corregir las cantidades, no coincide con la cantidad solicitada.');
            } else {
                $("#lbl_MensajeConfigLote").text('');
                $("#btnConfLote").show();
            }

        });
    }


);

