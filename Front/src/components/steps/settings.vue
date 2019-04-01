<template>
	<universal-container 
		:backButtonRequired="true" 
		:forwardButtonRequired="true" 
		:forwardButtonStatus="forwardButtonStatus"
		:stepperStatus="3">
		<h2 slot="title">{{ title }}</h2>
		<div slot="content">
		</div>
		<div slot="backButton">
			{{ backButtonText }}
		</div>
		<div slot="forwardButton">
			{{ forwardButtonText }}
		</div>
	</universal-container>
</template>

<script>
import universalContainer from '@/components/universal/universalContainer'

export default {
	name: 'Settings',
	components: {
		universalContainer,
	},
	data: function () {
		return {
			title: 'Настройки обзвона',
			backButtonText: 'Назад к синтезу речи',
			forwardButtonText: 'Запуск обзвона !',
			forwardButtonStatus: true
		}
	}
}
</script>

<style scoped lang="scss">
.router-link {
	color: white;
}
</style>