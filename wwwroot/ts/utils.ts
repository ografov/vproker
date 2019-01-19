namespace vproker {
    export class Utils {
        static dateTimeFormatter(value) {
            if (!value) {
                return "-";
            }
            var date = new Date(value + 'Z');
            return '<span title="' + date + '">' + date.toLocaleString('ru-RU') + '</span>';
        }

        static dateFormatter(value) {
            if (!value) {
                return "-";
            }
            var date = new Date(value + 'Z');
            return '<span title="' + date + '">' + date.toLocaleDateString('ru-RU') + '</span>';
        }

        static getClientActions(id) {
            return Utils.getActions({ controller: "Client", id });
        }

        static getMaintainActions(id, row, index) {
            return Utils.getActions({ controller: "Maintain", id, hasClose: !row.isFinished });
        }

        static getActions(options: { controller: string, id: string, hasClose?: boolean }) {

            const { controller, id, hasClose } = options;
            const closeBtn =
                //`<a  href="/${controller}/Close/${id}" title="Закрыть" class="btn btn-success btn-sm" title="Закрыть" ><span class="glyphicon glyphicon-ok"></span></a>`;
                `<a class="" href="/${controller}/Close/${id}" title="Закрыть" style="margin: 3px"><i class="glyphicon glyphicon-ok"></i></a>`;
            const editBtn =
                // `<a asp-action="Edit" href="/${controller}/Edit/${id}" title="Редактировать" class="btn btn-warning btn-sm" title="Редактировать"><span class="glyphicon glyphicon-edit"></span></a>`;
                `<a class="edit" href="/${controller}/Edit/${id}" title="Редактировать" style="margin: 3px"><i class="glyphicon glyphicon-edit"></i></a>`;
            const detailsBtn =
                // `<a class="btn btn-info btn-sm" href="/${controller}/Details/${id}" title="Просмотр" title="Просмотр" ><span class="glyphicon glyphicon-info-sign"></span></a>`;
                `<a class="info-sign" href="/${controller}/Details/${id}" title="Просмотр" style="margin: 3px"><i class="glyphicon glyphicon-info-sign"></i></a>`;
            const deleteBtn =
                // `<a class="btn btn-danger btn-sm" href="/${controller}/Delete/${id}" title="Удалить" title="Удалить"><span class="glyphicon glyphicon-remove"></span></a>`;
                `<a class="edit" href="/${controller}/Delete/${id}" title="Удалить" style="margin: 3px"><i class="glyphicon glyphicon-remove"></i></a>`;

            return (hasClose ? closeBtn : '') + editBtn + detailsBtn + deleteBtn;
        }
    }
}