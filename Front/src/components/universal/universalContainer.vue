<template>
	<div>
		<v-stepper value="3" v-model="stepperStatus">
			<v-stepper-header>
				<v-stepper-step :step="fileUploaded" :complete="stepperStatus > fileUploaded">Исходные данные</v-stepper-step>
				<v-divider></v-divider>
				<v-stepper-step :step="voiceGenerated" :complete="stepperStatus > voiceGenerated">Создание сообщения</v-stepper-step>
				<v-divider></v-divider>
				<v-stepper-step :step="eventSetup" :complete="stepperStatus > eventSetup">Настройки</v-stepper-step>
			</v-stepper-header>
		</v-stepper>
		
		<v-container grid-list-md>
			<v-layout row class="mt-2">
				<div class="title">
					<slot name="title"></slot>
				</div>
			</v-layout>
			<v-divider class="mt-2"></v-divider>
			<v-layout row>
				<div class="content">
					<slot name="content"></slot>
				</div>
			</v-layout>
			<v-layout row>
				<v-flex sm6 md4>
					<v-btn color="primary" :ripple="true" :block="true" 
						v-show="backButtonRequired"
						@click="backStep">
						<slot name="backButton"></slot>
					</v-btn>
				</v-flex>
				<v-flex sm6 offset-md4 md4>
					<v-btn color="primary" :ripple="true" :block="true" 
						v-show="forwardButtonRequired"
						v-bind:disabled="!forwardButtonStatus"
						@click="nextStep">
						<slot name="forwardButton"></slot>
					</v-btn>
				</v-flex>
			</v-layout>
		</v-container>
	</div>
</template>

<script>
export default {
	name: 'universalContainer',
	props: {
		backButtonRequired: {
			type: Boolean,
			default: false
		},
		forwardButtonRequired: {
			type: Boolean,
			default: true
		},
		forwardButtonStatus: {
			type: Boolean,
			default: false
		},
		stepperStatus: {
			type: Number,
			default: 1
		},
	},
	data: function () {
		return {
			fileUploaded: 1,
			voiceGenerated: 2,
			eventSetup: 3
		}
	},
	methods: {
		nextStep () {
			this.$root.$emit('events_step_forward', this.stepperStatus)
		},
		backStep () {
			this.$root.$emit('events_step_backward', this.stepperStatus)
		}
	}
}
</script>

<style scoped>
	.title, .content {
		text-align: left;
	}
	.content {
		min-height: 500px;
	}
</style>
