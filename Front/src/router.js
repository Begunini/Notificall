import Vue from 'vue'
import Router from 'vue-router'
import Dashboard from '@/components/dashboard'
import Events from '@/components/events'

Vue.use(Router)

export default new Router({
	mode: 'history',
	base: '/',
	routes: [
		{
			path: '/',
			name: 'start',
			component: Dashboard
		},
		{
			path: '/dashboard',
			name: 'dashboard',
			component: Dashboard
		},
		{
			path: '/events/add',
			name: 'events',
			component: Events
		}
	]
})
