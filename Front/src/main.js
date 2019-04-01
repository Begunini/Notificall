// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import router from './router'
import store from './store'
import Vuetify from 'vuetify'
import App from './App'

import 'vuetify/dist/vuetify.css'

Vue.use(Vuetify)
Vue.config.productionTip = false

/* eslint-disable no-new */
export default new Vue({
	el: '#app',
	store,
	router,
	components: { App },
	template: '<App/>'
})