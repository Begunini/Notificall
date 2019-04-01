<template>
	<universal-container 
		:backButtonRequired="false" 
		:forwardButtonRequired="true" 
		:forwardButtonStatus="forwardButtonStatus"
		:stepperStatus="1">
		<h2 slot="title">{{ title }}</h2>
		<div slot="content">
			<v-layout row wrap class="mt-2">
				<v-flex xs12 sm6 md5 lg4>
					<v-layout row wrap>
						<v-flex xs12>
							<v-card>
								<v-card-title class="pt-2 pb-1">
									<h3 class="headline grey--text text--darken-3">
										Существующий список
									</h3>
									<div>Выберите базу из выпадающего списка.</div>
								</v-card-title>
								<v-card-actions class="pl-3 pr-3">
									<v-select
										v-bind:items="sources"
										v-model="sourceSelected"
										item-text="title"
										item-value="id"
										label="Название базы"
										>
									</v-select>
								</v-card-actions>
							</v-card>
						</v-flex>
						<v-flex xs12 class="mt-2">
							<div class="title grey--text text--darken-1 text-xs-center">Или</div>
						</v-flex>
						<v-flex xs12>
							<v-card class="mt-2">
								<v-card-title class="pt-2 pb-1">
									<h3 class="headline grey--text text--darken-3">
										Загрузить файл
									</h3>
									<div>База номеров и дополнительные данные будут взяты из загруженного файла.</div>
								</v-card-title>
								<v-card-actions class="pl-3 pr-3 pb-3">
									<v-layout row wrap>
										<v-flex xs12>
											<v-text-field
												label="Название базы"
												v-model="upload.name"
												:error="upload.nameErrorState"
												:hide-details="!upload.nameErrorState"
												:error-messages="upload.nameError"
											></v-text-field>
										</v-flex>
										<v-flex xs12 v-if="!upload.correctFileFormat" class="mt-2">
											<v-alert :value="true" color="error" icon="error">
												Неправильный формат файла
											</v-alert>
										</v-flex>
										<v-flex xs12 class="mt-2">
											<div class="dropbox" :class="{'hovered': upload.drag}">
												<input type="file" 
												@change="uploadFileChanged($event.target.files)"
												@dragenter="upload.drag = true" 
												@dragleave="upload.drag = false"
												ref="uploadFile"
												class="input-file"
												accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel,
												text/plain"
												>
												<p class="input-container" v-if="!upload.file">
													<v-icon dark>move_to_inbox</v-icon>
													<br>Перетащите сюда файл
													<br>или кликните для выбора
												</p>
												<p class="input-container" v-if="upload.file">
													<v-icon dark>attach_file</v-icon>
													{{ upload.file.name }}
												</p>
											</div>
										</v-flex>
										<v-flex xs8 offset-xs4 class="mt-2">
											<v-btn
												:loading="upload.active"
												:disabled="!uploadEnabled"
												color="blue-grey"
												class="white--text"
												:block="true"
												@click="uploadFile"
												>
												Загрузить файл
												<v-icon right dark>cloud_upload</v-icon>
											</v-btn>
										</v-flex>
									</v-layout>
								</v-card-actions>
							</v-card>
						</v-flex>
					</v-layout>
				</v-flex>
				<v-flex xs12 sm6 md7 lg8>
					<v-card class="tableCard">
						<v-card-title class="pt-2 pb-1">
							<h3 class="headline grey--text text--darken-3">
								Данные из выбранной базы
							</h3>
						</v-card-title>
						<v-card-actions>
							<v-layout row wrap class="table">
								<v-flex xs12>
									<v-data-table
										:headers="table.headers"
										:items="table.items"
										:loading="table.loading"
										:hide-headers="table.hideBlocks"
										hide-actions
										class="table"
										>
										<template slot="items" slot-scope="content">
											<td v-for="(value, index) in content.item" :key="index">{{ value }}</td>
										</template>
										<template slot="no-data">
											<v-alert :value="true" color="primary" icon="warning" v-if="table.hideBlocks">
												Загрузите файл или выберите базу для отображения данных
											</v-alert>
										</template>
									</v-data-table>
								</v-flex>
								<v-flex xs12>
									<div class="text-xs-center pt-2">
										<v-pagination v-model="pagination" :length="tablePages"></v-pagination>
									</div>
								</v-flex>
								<v-flex xs8 offset-xs4>
									<v-select
										v-bind:items="phoneColumns"
										v-model="phoneColumnSelected"
										item-text="title"
										item-value="index"
										label="Столбец с номером телефона"
										>
									</v-select>
								</v-flex>
							</v-layout>
						</v-card-actions>
					</v-card>
				</v-flex>
			</v-layout>
		</div>
		<div slot="forwardButton">{{ forwardButtonText }}</div>
	</universal-container>
</template>

<script>
import universalContainer from '@/components/universal/universalContainer'
import axios from 'axios'
import { mapActions } from 'vuex'

export default {
	name: 'UploadSourceFile',
	data: function () {
		return {
			sources: [],
			sourceSelected: null,
			phoneColumns: [],
			phoneColumnSelected: null,
			upload: {
				drag: false,
				file: null,
				name: '',
				nameError: [],
				nameErrorText: 'База с таким имененм уже существует',
				nameErrorState: false,
				active: false,
				correctFileFormat: true
			},
			table: {
				headers: [],
				items: [],
				totalItems: 0,
				hideBlocks: true,
				loading: false,
				rowsPerPage: 5
			},
			pagination: 1,
			tableLoading: false,
			title: 'Выбор источника данных',
			forwardButtonText: 'К синтезу речи',
			forwardButtonStatus: false
		}
	},
	computed: {
		tablePages () {
			return Math.ceil(this.table.totalItems / 5)
		},
		uploadEnabled () {
			if (this.upload.name.length !== 0 && this.upload.file !== null) {
				return true
			}
			return false
		}
	},
	components: {
		universalContainer,
	},
	mounted () {
		this.getSources()
	},
	watch: {
		tableLoading (val) {
			this.table.hideBlocks = false
			this.table.loading = val
		},
		sourceSelected (val) {
			this.table.headers = []
			this.table.items = []
			this.phoneColumns = []
			
			this.getSourceData()

			this.setSourceId(val)
		},
		pagination () {
			this.getSourceData()
		},
		phoneColumnSelected (val) {
			axios({
				method: 'post',
				url: 'http://localhost:50481/api/sources/' + this.sourceSelected + '/phone-column',
				data: {
					columnIndex: val
				}
			}).then(() => {
				this.forwardButtonStatus = true	
			}).catch(error => {
				console.log(error)
			})
		}
	},
	methods: {
		...mapActions([
			'setSourceId'
		]),
		getSources () {
			return new Promise((resolve, reject) => {
				axios.get('http://localhost:50481/api/sources')
					.then(response => {
						this.sources = response.data
					})
					.catch(error => {
						console.log(error)
					})
				resolve()
			})
		},
		uploadFileChanged (files) {
			this.upload.correctFileFormat = true
			this.upload.file = _.first(files)
		},
		uploadFile () {
			this.upload.active = true

			return new Promise((resolve, reject) => {
				let formData = new FormData()
				formData.append('name', this.upload.name)
				formData.append('source', this.upload.file)

				axios.post('http://localhost:50481/api/sources/upload', formData)
					.then(response => {
						resolve(response.data)
					})
					.catch(error => {
						switch (error.response.status) {
						case 415:
							this.upload.file = null
							this.upload.correctFileFormat = false
							this.$refs.uploadFile.type = 'text'
							this.$refs.uploadFile.type = 'file'
							break
						case 409:
							this.upload.nameErrorState = true
							this.upload.nameError.push(this.upload.nameErrorText)
							this.upload.active = false
							break
						}
						this.upload.active = false
						reject(error.response)
					})
			}).then(sourceId => {
				this.flushFileUpload()

				this.getSources().then(() => {
					this.sourceSelected = sourceId
				})
			})
		},
		flushFileUpload () {
			this.upload.correctFileFormat = true
			this.upload.active = false
			this.upload.name = ''
			this.upload.nameErrorState = false
			this.upload.nameErrorText = []
			this.upload.file = null
			this.$refs.uploadFile.type = 'text'
			this.$refs.uploadFile.type = 'file'
		},
		getSourceData () {
			this.tableLoading = true

			let offset = (this.pagination - 1) * this.table.rowsPerPage

			axios.get('http://localhost:50481/api/sources/' + this.sourceSelected + '/headers').then(response => {
				this.table.headers = response.data.map(header => {
					return {
						sortable: false, 
						text: header, 
						align: 'left'
					}
				})
				this.phoneColumns = response.data.map((header, index, headers) => {
					return {
						title: header,
						index: index
					}
				})
			}).catch(error => {
				console.log(error)
			})
			let query = 'rows=' + this.table.rowsPerPage + '&offset=' + offset
			axios.get('http://localhost:50481/api/sources/' + this.sourceSelected + '/items?' + query).then(response => {
				let data = response.data
				this.table.totalItems = data.total
				this.table.items = data.items

				this.tableLoading = false
			}).catch(error => {
				console.log(error)
			})
			axios.get('http://localhost:50481/api/sources/' + this.sourceSelected + '/phone-column').then(response => {
				if (typeof response.data === 'number') {
					this.phoneColumnSelected = response.data
					this.forwardButtonStatus = true	
				}
			}).catch(error => {
				console.log(error)
			})
		},
	}
}
</script>

<style scoped lang="scss">
h1, h2 {
	font-weight: normal;
}
.tableCard {
	height: 100%!important;

	.table {
		width: 100%;
	}
}
.dropbox {
	border-radius: 10px;
    border: 2px dashed grey;
	min-height: 150px;
	position: relative;
	cursor: pointer;
	display: table;
	width: 100%;

	&:hover {
		background: rgba(173, 216, 230, 0.25);
	}

	&.hovered {
		background: rgba(173, 216, 230, 0.25);
	}

	.input-file {
		opacity: 0;
		width: 100%;
		height: 150px;
		position: absolute;
		cursor: pointer;
	}

	.input-container {
		font-size: 1.2em;
		text-align:center;
		vertical-align: middle;
		display: table-cell;

		.icon {
			color: rgba(0,0,0,0.54);
			font-size: 3rem;
		}
	}
}
</style>
