
Vue.use( VueRouter );

var MaintainApi = {
    fetchSync: function () {
        $.ajax({
            async: false,
            url: "/api/maintain",
            type: 'GET',
            success: function (response) {
                debugger;
                console.log(response);
                Maintains = response;
            }.bind(this)
        })
    },
    store: function (item) {
        $.ajax({
            async: false,
            url: "/api/maintain/store",
            type: 'POST',
            success: function () {
                console.log('saved');
                Maintains = response;
            }.bind(this)
        })
    }
}

function maintainInit() {
    MaintainApi.fetchSync();
    var router = new VueRouter({
        routes: [
            { path: '/', component: List },
            { path: '/item/:item_id', component: Item, name: 'item' },
            { path: '/add-item', component: AddItem },
            { path: '/item/:item_id/edit', component: ItemEdit, name: 'item-edit' },
            { path: '/item/:item_id/delete', component: ItemDelete, name: 'item-delete' }
        ]
    });
    app = new Vue({
        router: router
    }).$mount('#app')
}

var MaintainStorage = {
    fetch: function () {
        return Maintains;
    },
    save: function (items) {
        Maintains = items;
    },
    add: function (item) {
        Maintains.push(item);
    },
    update: function (item) {
        Maintains[findItemKey(item.id)] = item;
    },
    delete: function (id) {
        Maintains.splice(findItemKey(id), 1);
    }
}

var Maintains = [
  //{id: 1, name: 'Чистка', description: 'Lorem, ipsum dolor sit amet consectetur adipisicing elit.', date: '15.01.2018'},
  //{id: 2, name: 'Разбор', description: 'Lorem, ipsum dolor sit amet consectetur adipisicing elit.', date: '15.03.2018'},
  //{id: 3, name: 'Ремонт', description: 'Lorem, ipsum dolor sit amet consectetur adipisicing elit.', date: '15.05.2018'}
];

function findItem (itemId) {
  return Maintains[findItemKey(itemId)];
};

function findItemKey (itemId) {
  for (var key = 0; key < Maintains.length; key++) {
    if (Maintains[key].id == itemId) {
      return key;
    }
  }
};

var List = Vue.extend({
  template: '#item-list',
  data: function () {
      return {
          items: MaintainStorage.fetch(),
          searchKey: ''
      };
  },

  // watch todos change for localStorage persistence
  //watch: {
  //    items: {
  //        deep: true,
  //        handler: MaintainStorage.save
  //    }
  //},

  computed: {
    filteredItems: function () {
      return this.items.filter(function (item) {
        return this.searchKey=='' || item.name.indexOf(this.searchKey) !== -1;
      },this);
    }
  }
});

var Item = Vue.extend({
  template: '#item',
  data: function () {
    return {item: findItem(this.$route.params.item_id)};
  }
});

var ItemEdit = Vue.extend({
  template: '#item-edit',
  data: function () {
    return {item: findItem(this.$route.params.item_id)};
  },
  methods: {
    updateItem: function () {
      var item = this.item;
      MaintainStorage.update({
            id: item.id,
            name: item.name,
            description: item.description,
            date: item.date
      });
      router.push('/');
    }
  }
});

var ItemDelete = Vue.extend({
  template: '#item-delete',
  data: function () {
    return {item: findItem(this.$route.params.item_id)};
  },
  methods: {
      deleteItem: function () {
          MaintainStorage.delete(this.$route.params.item_id);
          router.push('/');
      }
  }
});

var AddItem = Vue.extend({
  template: '#add-item',
  data: function () {
    return {item: {name: '', description: '', date: ''}}
  },
  methods: {
      createItem: function () {
          var item = this.item;
          MaintainStorage.add({
              id: Math.random().toString().split('.')[1],
              name: item.name,
              description: item.description,
              date: item.date
          });
          router.push('/');
      }
  }
});

