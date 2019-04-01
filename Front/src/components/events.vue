<template>
	<keep-alive>
		<component v-bind:is="currentView"></component>
	</keep-alive>
</template>

<script>
import axios from 'axios'
import sourceSelect from '@/components/steps/source'
import speechSelect from '@/components/steps/speech'
import settings from '@/components/steps/settings'

export default {
	name: 'Events',
	data: function () {
		return {
			currentView: 'sourceSelect'
		}
	},
	components: {
		sourceSelect,
		speechSelect,
		settings
	},
	mounted () {
		let that = this
		this.$root.$on('events_step_forward', function (val) {
			switch (val) {
			case 1:
				that.currentView = 'speechSelect'
				break
			case 2:
				that.currentView = 'settings'
				break
			case 3:
				let query = '?sourceId=' + this.$store.getters.sourceId + '&textId=' + this.$store.getters.textId
				axios.post('http://localhost:50481/api/events' + query).then(() => {
					this.$router.push({ name: 'dashboard' })
				})
				break
			}
		})
		this.$root.$on('events_step_backward', function (val) {
			switch (val) {
			case 2:
				that.currentView = 'sourceSelect'
				break
			case 3:
				that.currentView = 'speechSelect'
				break
			}
		})
	}
}
</script>

<style scoped>

</style>
