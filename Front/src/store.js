import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
	state: {
		sourceId: null,
		textId: null
	},
	getters: {
		sourceId: state => state.sourceId,
		textId: state => state.textId
	},
	actions: {
		setSourceId: ({ commit }, sourceId) => {
			commit('setSourceId', sourceId)
		},
		setTextId: ({ commit }, textId) => {
			commit('setTextId', textId)
		}
	},
	mutations: {
		setSourceId (state, sourceId) {
			state.sourceId = sourceId
		},
		setTextId (state, textId) {
			state.textId = textId
		}
	}
})