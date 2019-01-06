namespace vproker {
export class Utils {
        static dateTimeFormatter(value) {
            var date = new Date(value + 'Z');
            return '<span title="' + date + '">' + date.toLocaleString('ru-RU') + '</span>';
        }

        static dateFormatter(value) {
            var date = new Date(value + 'Z');
            return '<span title="' + date + '">' + date.toLocaleDateString('ru-RU') + '</span>';
        }
        static getClientActions(id) {

            var editBnt = '<a class="edit" href="/Client/Edit/' + id + '" title="Редактировать" style="margin: 3px"><i class="glyphicon glyphicon-edit"></i></a>';
            var detailsBtn = '<a class="info-sign" href="/Client/Details/' + id + '" title="Просмотр" style="margin: 3px"><i class="glyphicon glyphicon-info-sign"></i></a>';
            var deleteBtn = '<a class="edit" href="/Client/Delete/' + id + '" title="Удалить" style="margin: 3px"><i class="glyphicon glyphicon-remove"></i></a>';

            return editBnt + detailsBtn + deleteBtn;
        }
}
}