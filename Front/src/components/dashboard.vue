<template>
	<v-container grid-list-md>
		<v-layout row class="mt-2">
			<div class="title">
				<h2>Информационная панель</h2>
			</div>
		</v-layout>
		<v-divider class="mt-2"></v-divider>
		<v-layout row class="mt-2">
			<div class="content">
				<v-layout row wrap>
					<v-flex xs12>
						<v-card class="event-history">
							<v-card-title class="pt-2 pb-1">
								<h3 class="headline grey--text text--darken-3">
									История автообзвонов
								</h3>
							</v-card-title>
							<v-card-actions class="pl-3 pr-3">
								<v-expansion-panel>
									<v-expansion-panel-content v-for="(event, index) in events" :key="index">
									<div slot="header">
										<v-layout row wrap>
											<v-flex xs4>
												<v-chip label v-if="event.status === 0" color="primary" text-color="white">Ожидает обработки</v-chip>
												<v-chip label v-if="event.status === 1" color="orange" text-color="white">В процессе</v-chip>
												<v-chip label v-if="event.status === 2" color="red" text-color="white">Приостановлено</v-chip>
												<v-chip label v-if="event.status === 3" color="green" text-color="white">Завершено</v-chip>
											</v-flex>
											<v-flex xs4>
												<div class="title">Создан:</div>{{formattedDate(event.created)}}
											</v-flex>
											<v-flex xs4>
												<div class="title">Последнее изменение:</div>{{formattedDate(event.edited)}} 
											</v-flex>
											<v-flex xs4 offset-xs4>
												<div class="title">Источник данных:</div>{{event.source}}
											</v-flex>
											<v-flex xs4>
												<div class="title">Текстовка:</div>{{event.text}} 
											</v-flex>
										</v-layout>
									</div>
									<v-list>
										<template v-for="(call, index) in event.calls">
											<v-list-tile :key="index">
												<v-chip label v-if="call.result === 0" color="primary" text-color="white">Ожидает обработки</v-chip>
												<v-chip label v-if="call.result === 1" color="green" text-color="white">Успешно</v-chip>
												<v-chip label v-if="call.result === 2" color="red" text-color="white">Неуспешно</v-chip>
											<v-list-tile-content>
												<v-list-tile-title v-html="call.phone"></v-list-tile-title>
												<v-list-tile-sub-title v-html="call.messageText"></v-list-tile-sub-title>
											</v-list-tile-content>
											</v-list-tile>
										</template>
									</v-list>
									</v-expansion-panel-content>
								</v-expansion-panel>
								<div v-for="(event, index) in events" v-bind:key="index">
									
								</div>
							</v-card-actions>
						</v-card>
					</v-flex>
				</v-layout>
			</div>
		</v-layout>
	</v-container>
</template>

<script>
import axios from 'axios'
import moment from 'moment'

export default {
	name: 'Dashboard',
	data () {
		return {
			events: []
		}
	},
	mounted () {
		moment.locale('ru')
		setInterval(() => {
			this.getEvents()
		}, 2500)
	},
	methods: {
		getEvents () {
			axios.get('http://localhost:50481/api/events').then(response => {
				this.events = response.data
			})
		},
		formattedDate (unformatted) {
			return moment(unformatted).format('lll')
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
	.event-history {
		min-height: 500px;
	}
</style>
