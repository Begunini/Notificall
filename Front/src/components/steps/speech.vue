<template>
	<universal-container 
		:backButtonRequired="true" 
		:forwardButtonRequired="true" 
		:forwardButtonStatus="forwardButtonStatus"
		:stepperStatus="2">
		<h2 slot="title">{{ title }}</h2>
		<div slot="content">
			<v-layout row wrap class="mt-2">
				<v-flex xs12>
					<v-card>
						<v-card-title>
							<h3 class="headline grey--text text--darken-3">
								Выберите тип голосового сообщения
							</h3>
						</v-card-title>
						<v-card-text class="pt-0">
							<v-radio-group v-model="speechSource" :mandatory="true" :hide-details="true" row class="pt-0">
								<v-radio label="Текст" value="1"></v-radio>
								<v-radio label="Текст + фоновая музыка" value="2"></v-radio>
								<v-radio label="Сохраненная комбинация" value="3"></v-radio>
							</v-radio-group>
						</v-card-text>
					</v-card>
				</v-flex>
			</v-layout>
			<v-layout row wrap class="mt-1">
				<v-flex xs12>
					<v-card>
						<v-card-actions class="pl-3 pr-3">
							<v-layout row wrap>
								<v-flex xs12 v-if="speechSource === '3'">
									<v-layout row wrap>
										<v-flex xs6 class="pt-4">
											<h3 class="headline grey--text text--darken-3">
												Выберите готовое сообщение
											</h3>
										</v-flex>
										<v-flex xs6>
											<v-select
												v-bind:items="combination.items"
												v-model="combination.selected"
												item-text="title"
												item-value="id"
												label="Название существующей комбинации"
												>
											</v-select>
										</v-flex>
									</v-layout>
									<v-divider></v-divider>
								</v-flex>
								<v-flex xs12>
									<v-layout row wrap>
										<v-flex xs6>
											<h3 class="headline grey--text text--darken-3">
												Текст сообщения
											</h3>
											<v-radio-group v-model="text.source" :mandatory="true" :hide-details="true" class="pt-0">
												<v-radio label="Набрать текст" value="1"></v-radio>
												<v-radio label="Существующий текст" value="2"></v-radio>
												<v-radio label="Загрузить новую голосовую аудиозапись" value="3"></v-radio>
												<v-radio label="Существующая голосовая аудиозапись" value="4"></v-radio>
											</v-radio-group>
										</v-flex>
										<v-flex xs6 style="margin-bottom: 70px">
											<v-layout row wrap>
												<v-flex xs12 v-if="text.source === '1'">
													<div contenteditable="true">
														<v-chip outline color="primary" class="textToken" contenteditable="false" v-for="(header, index) in text.headers" v-bind:key="index">{{header}}</v-chip>
													</div>
													<div class="text textInput textTokenDropzone mt-1" contenteditable="true" ref="textInput">
														Введите текст и вставьте указатели
													</div>
												</v-flex>
												<v-flex xs12 v-if="text.source === '2'">
													<v-select
														v-bind:items="text.items"
														v-model="textSelected"
														item-text="title"
														item-value="id"
														label="Выберите готовый текст"
														:hide-details="true"
														>
													</v-select>
													<div class="text" v-html="text.selectedHtml" contenteditable="false">
													</div>
												</v-flex>
												<v-flex xs12 v-if="text.source === '3'">
													<v-layout row wrap>
														<v-flex xs12>
															<v-text-field
																label="Название аудиосообщения"
																v-model="text.upload.name"
																:error="text.upload.nameErrorState"
																:hide-details="!text.upload.nameErrorState"
																:error-messages="text.upload.nameError"
															></v-text-field>
														</v-flex>
														<v-flex xs12 v-if="!text.upload.correctFileFormat" class="mt-2">
															<v-alert :value="true" color="error" icon="error">
																Неправильный формат файла
															</v-alert>
														</v-flex>
														<v-flex xs12 class="mt-2">
															<div class="dropbox" :class="{'hovered': text.upload.drag}">
																<input type="file" 
																@change="textFileChanged($event.target.files)"
																@dragenter="text.upload.drag = true" 
																@dragleave="text.upload.drag = false"
																ref="uploadFile"
																class="input-file"
																accept="audio/*"
																>
																<p class="input-container" v-if="!text.upload.file">
																	<v-icon dark>move_to_inbox</v-icon>
																	<br>Перетащите сюда файл
																	<br>или кликните для выбора
																</p>
																<p class="input-container" v-if="text.upload.file">
																	<v-icon dark>attach_file</v-icon>
																	{{ text.upload.file.name }}
																</p>
															</div>
														</v-flex>
														<v-flex xs8 offset-xs4 class="mt-2">
															<v-btn
																:loading="text.upload.active"
																:disabled="!textUploadEnabled"
																color="blue-grey"
																class="white--text"
																:block="true"
																@click="textUpload"
																>
																Загрузить файл
																<v-icon right dark>cloud_upload</v-icon>
															</v-btn>
														</v-flex>
													</v-layout>
												</v-flex>
												<v-flex xs12 v-if="text.source === '4'">
													<v-select
														v-bind:items="text.audios.items"
														v-model="text.audios.selected"
														item-text="title"
														item-value="id"
														label="Название аудиодорожки"
														>
													</v-select>
												</v-flex>
											</v-layout>
										</v-flex>
										<v-flex xs12>
											<v-layout row wrap>
												<v-flex xs6 offset-xs6 v-if="(speechSource === '1' || speechSource === '2') && (text.source === '1' || text.source === '2')">
													<v-layout row wrap>
														<v-flex xs6>
															<v-select
															:items="text.languages"
															item-text="title"
															item-value="value"
															v-model="text.languageValue"
															label="Язык"
															:disabled="!textParametersEditEnabled"
															></v-select>
														</v-flex>
														<v-flex xs6>
															<v-select
															:items="text.speakers"
															item-text="title"
															item-value="value"
															v-model="text.speakerValue"
															label="Диктор"
															:disabled="!textParametersEditEnabled"
															></v-select>
														</v-flex>
														<v-flex xs6>
															<v-select
															:items="text.emotions"
															item-text="title"
															item-value="value"
															v-model="text.emotionValue"
															label="Характер"
															:disabled="!textParametersEditEnabled"
															></v-select>
														</v-flex>
														<v-flex xs12>
															<v-layout row wrap>
																<v-flex xs10>
																	<v-slider label="Скорость речи, %" :min="10" :max="300" v-model="text.speedValue" :disabled="!textParametersEditEnabled"></v-slider>
																</v-flex>
																<v-flex xs2>
																	<v-text-field v-model="text.speedValue" :disabled="!textParametersEditEnabled" type="number"></v-text-field>
																</v-flex>
															</v-layout>
														</v-flex>
													</v-layout>
												</v-flex>
											</v-layout>
										</v-flex>
										<v-flex xs12 v-if="(speechSource === '1' || speechSource === '2') && text.source === '1'">
											<v-layout row wrap>
												<v-flex xs4 offset-xs8 class="pl-2 pr-0">
													<v-text-field
														label="Название текстовки"
														v-model="text.save.name"
														:error="text.save.nameErrorRequired"
														:hide-details="!text.save.nameErrorRequired"
														:error-messages="text.save.nameError"
													></v-text-field>
												</v-flex>
												<v-flex xs4 offset-xs8>
													<v-btn
														:loading="text.save.active"
														:disabled="!textSaveEnabled"
														color="blue-grey"
														class="white--text"
														:block="true"
														@click="saveText"
														>
														Сохранить сообщение
														<v-icon right dark>save</v-icon>
													</v-btn>
												</v-flex>
											</v-layout>
										</v-flex>
									</v-layout>
								</v-flex>
								<v-flex xs12 v-if="speechSource === '2' || speechSource === '3'">
									<v-divider class="mt-1 mb-1"></v-divider>
									<v-layout row wrap>
										<v-flex xs6>
											<h3 class="headline grey--text text--darken-3">
												Фоновая музыка
											</h3>
											<v-radio-group v-model="background.source" :mandatory="true" :hide-details="true" class="pt-0">
												<v-radio label="Существующая фоновая музыка" value="1"></v-radio>
												<v-radio label="Загрузить новую" value="2"></v-radio>
											</v-radio-group>
										</v-flex>
										<v-flex xs6>
											<v-layout row wrap>
												<v-flex xs12 v-if="background.source === '1'">
													<v-select
														v-bind:items="background.items"
														v-model="background.selected"
														item-text="title"
														item-value="id"
														label="Название дорожки"
														>
													</v-select>
												</v-flex>
												<v-flex xs12 v-if="background.source === '2'">
													<v-layout row wrap>
														<v-flex xs12>
															<v-text-field
																label="Название аудиодорожки"
																v-model="background.upload.name"
																:error="background.upload.nameErrorState"
																:hide-details="!background.upload.nameErrorState"
																:error-messages="background.upload.nameError"
															></v-text-field>
														</v-flex>
														<v-flex xs12 v-if="!background.upload.correctFileFormat" class="mt-2">
															<v-alert :value="true" color="error" icon="error">
																Неправильный формат файла
															</v-alert>
														</v-flex>
														<v-flex xs12 class="mt-2">
															<div class="dropbox" :class="{'hovered': background.upload.drag}">
																<input type="file" 
																@change="backgroundFileChanged($event.target.files)"
																@dragenter="background.upload.drag = true" 
																@dragleave="background.upload.drag = false"
																ref="uploadFile"
																class="input-file"
																accept="audio/*"
																>
																<p class="input-container" v-if="!background.upload.file">
																	<v-icon dark>move_to_inbox</v-icon>
																	<br>Перетащите сюда файл
																	<br>или кликните для выбора
																</p>
																<p class="input-container" v-if="background.upload.file">
																	<v-icon dark>attach_file</v-icon>
																	{{ background.upload.file.name }}
																</p>
															</div>
														</v-flex>
														<v-flex xs8 offset-xs4 class="mt-2">
															<v-btn
																:loading="background.upload.active"
																:disabled="!backgroundUploadEnabled"
																color="blue-grey"
																class="white--text"
																:block="true"
																@click="backgroundUpload"
																>
																Загрузить файл
																<v-icon right dark>cloud_upload</v-icon>
															</v-btn>
														</v-flex>
													</v-layout>
												</v-flex>
											</v-layout>
										</v-flex>
									</v-layout>
								</v-flex>
								<v-flex xs12 v-if="speechSource === '2'">
									<v-layout row wrap>
										<v-flex xs4 offset-xs8 class="pl-2 pr-0">
											<v-text-field
												label="Название комбинации"
												v-model="combination.name"
												:error="combination.nameErrorRequired"
												:hide-details="!combination.nameErrorRequired"
												:error-messages="combination.nameError"
											></v-text-field>
										</v-flex>
										<v-flex xs4 offset-xs8>
											<v-btn
												:loading="combination.save.active"
												:disabled="!combinationSaveEnabled"
												color="blue-grey"
												class="white--text"
												:block="true"
												@click="saveCombination"
												>
												Сохранить комбинацию
												<v-icon right dark>save</v-icon>
											</v-btn>
										</v-flex>
									</v-layout>
								</v-flex>
							</v-layout>
						</v-card-actions>
					</v-card>
				</v-flex>
				<v-flex xs12 class="mt-1">
					<v-layout row wrap>
						<v-flex xs12>
							<v-card>
								<v-card-title class="pt-2 pb-1">
									<h3 class="headline grey--text text--darken-3">
										Предварительный результат
									</h3>
								</v-card-title>
								<v-card-actions class="pl-3 pr-3">
									<v-layout row wrap>
										<v-flex xs6>
											<v-data-table
												:headers="preview.headers"
												:items="preview.items"
												:loading="preview.tableLoading"
												hide-actions
												class="table"
												>
												<template slot="items" slot-scope="content">
													<td v-for="(value, index) in content.item" :key="index">{{ value }}</td>
												</template>
											</v-data-table>
											<div class="text-xs-center pt-2">
												<v-pagination v-model="preview.pagination" :length="preview.sourceItems"></v-pagination>
											</div>
										</v-flex>
										<v-flex xs6>
											<v-btn
												:loading="preview.isLoading"
												color="blue-grey"
												class="white--text"
												:block="true"
												@click="getPreview"
												:disabled="!previewEnabled"
												>
												Прослушать сообщение
												<v-icon right dark>save</v-icon>
											</v-btn>
										</v-flex>
									</v-layout>
								</v-card-actions>
							</v-card>
						</v-flex>
					</v-layout>
				</v-flex>
			</v-layout>
		</div>
		<div slot="backButton">{{ backButtonText }}</div>
		<div slot="forwardButton">{{ forwardButtonText }}</div>
	</universal-container>
</template>

<script>
import universalContainer from '@/components/universal/universalContainer'
import axios from 'axios'
import { mapGetters, mapActions } from 'vuex'

export default {
	name: 'UploadSourceFile',
	data: function () {
		return {
			firstLoad: true,
			speechSource: '1',
			textSelected: null,
			text: {
				source: '1',
				items: [],
				headers: [],
				selectedHtml: null,
				DD: {
					draggables: null,
					dropzones: null,
					dropLoad: null
				},
				upload: {
					drag: false,
					file: null,
					correctFileFormat: true,
					name: '',
					nameError: [],
					nameErrorText: 'Аудиодорожка с таким имененм уже существует',
					nameErrorState: false
				},
				save: {
					active: false,
					name: '',
					nameError: [],
					nameErrorText: 'Текст с таким имененм уже существует',
					nameErrorState: false
				},
				languages: [
					{
						title: 'Русский',
						value: 0
					},
					{
						title: 'Английский',
						value: 1
					},
				],
				languageValue: null,
				speakers: [
					{
						title: 'Жанна',
						value: 0
					},
					{
						title: 'Оксана',
						value: 1
					},
					{
						title: 'Алиса',
						value: 2
					},
					{
						title: 'Захар',
						value: 3
					},
					{
						title: 'Ермил',
						value: 4
					}
				],
				speakerValue: null,
				emotions: [
					{
						title: 'Веселый',
						value: 0
					},
					{
						title: 'Злой',
						value: 1
					},
					{
						title: 'Нейтральный',
						value: 2
					}
				],
				emotionValue: null,
				speedValue: 100,
				audios: {
					items: [],
					selected: null
				}
			},
			background: {
				source: '1',
				items: [],
				selected: null,
				upload: {
					drag: false,
					file: null,
					active: false,
					name: '',
					nameError: [],
					nameErrorText: 'Фоновая аудиодорожка с таким имененм уже существует',
					nameErrorState: false,
					correctFileFormat: true
				}
			},
			combination: {
				items: [],
				selected: null,
				save: {
					active: false,
				},
				name: '',
				nameError: [],
				nameErrorText: 'Комбинация с таким именем уже существует',
				nameErrorState: false,
			},
			preview: {
				headers: [],
				items: [],
				audioContext: null,
				pagination: 1,
				sourceItems: 0,
				tableLoading: false,
				isLoading: false
			},
			title: 'Текст сообщения и фоновая музыка',
			backButtonText: 'Назад к выбору источника',
			forwardButtonText: 'К настройкам обзвона',
		}
	},
	computed: {
		...mapGetters([
			'sourceId'
		]),
		textSourceSelected () {
			return this.text.source
		},
		textParametersEditEnabled () {
			if (this.text.source === '1') {
				return true
			} else if (this.text.source === '2') {
				return false
			}
			return false
		},
		textSaveEnabled () {
			if (this.text.save.name.length !== 0) {
				return true
			}
			return false
		},
		textUploadEnabled () {
			if (this.text.upload.name.length !== 0 && this.text.upload.file !== null) {
				return true
			}
			return false
		},
		backgroundUploadEnabled () {
			if (this.background.source === '2') {
				if (this.background.upload.name.length !== 0 && this.background.upload.file !== null) {
					return true
				}
			}
			return false
		},
		combinationSaveEnabled () {
			let textValid = false
			//check for text.source
			//validate selected

			let backgroundValid = false
			//check for background.source
			//validate selected

			if (textValid && backgroundValid && this.combination.name.length !== 0) {
				return true
			}
			return false
		},
		previewEnabled () {
			if (this.text.source === '1') {
				if (this.text.languageValue === null || this.text.speakerValue === null || this.text.emotionValue === null || this.text.speedValue < 10 || this.text.speedValue > 300) {
					return false
				}
			} else if (this.text.source === '2') {
				if (this.textSelected === null) {
					return false
				}
			} else if (this.text.source === '3') {
				return false
			} else if (this.text.source === '4') {
				if (this.text.audios.selected === null) {
					return false
				}
			}
			return true
		},
		previewOffset () {
			return this.preview.pagination - 1
		},
		forwardButtonStatus () {
			if (this.speechSource === '1') {
				if (this.text.source === '2') {
					if (this.textSelected !== null) {
						return true
					}
				}
			}
			return false
		},
	},
	components: {
		universalContainer,
	},
	mounted () {
		this.getTexts()

		this.getTextHeaders().then(() => {
			this.initPreview()
		})

		//get audios
		//get backgrounds
		//get combinations
	},
	updated () {
		this.initTextEdit()
	},
	watch: {
		speechSource (val) {
			if (val === '3') {
				this.text.source = '2'
				this.background.source = '1'
			} else {
				this.text.source = '1'
				this.background.source = '2'
			}
		},
		textSourceSelected (val) {
			if (val === '1') {
				this.initTextEdit()
				this.text.languageValue = null
				this.text.speakerValue = null
				this.text.emotionValue = null
				this.text.speedValue = 100
			}
		},
		textSelected (val) {
			axios.get('http://localhost:50481/api/texts/' + val)
				.then(response => {
					let regEx = /%{2}[А-ЯЁа-яёA-Za-z0-9-_]+%{2}/g
					let textEntity = response.data
					let text = textEntity.value.replace(regEx, '<span class="chip textToken chip--outline primary primary--text" contenteditable="false" style="height: 26px;"><span class="chip__content">$&</span></span>').replace(/%{2}/g, '')
					this.text.selectedHtml = text
					this.text.languageValue = textEntity.language
					this.text.speakerValue = textEntity.speaker
					this.text.emotionValue = textEntity.emotion
					this.text.speedValue = textEntity.speed * 100
				})
				.catch(error => {
					console.log(error.response)
				})
			this.setTextId(val)
		},
		previewOffset (val) {
			this.getPreviewItem(val)
		}
	},
	methods: {
		...mapActions([
			'setTextId'
		]),
		initTextEdit () {
			this.text.DD.draggables = $('.textToken').attr('draggable', 'true')
			this.text.DD.dropzones = $('.textTokenDropzone').attr('dropzone', 'copy')
			this.textBindDraggables()
			this.textBindDropzones()
		},
		textBindDraggables () {
			let that = this
			this.text.DD.draggables.off('dragstart').on('dragstart', function (event) {
				let e = event.originalEvent
				$(e.target).removeAttr('dragged')
				let dt = e.dataTransfer
				let content = e.target.outerHTML
				let isDraggable = that.text.DD.draggables.is(e.target)
				if (isDraggable) {
					dt.effectAllowed = 'copy'
					dt.setData('text/plain', ' ')
					that.text.DD.dropLoad = content
				}
			})
		},
		textBindDropzones () {
			let that = this
			this.text.DD.dropzones.off('dragleave').on('dragleave', function (event) {
				let e = event.originalEvent
				let dt = e.dataTransfer
				let relatedTarget_is_dropzone = that.text.DD.dropzones.is(e.relatedTarget)
				let relatedTarget_within_dropzone = that.text.DD.dropzones.has(e.relatedTarget).length > 0
				let acceptable = relatedTarget_is_dropzone || relatedTarget_within_dropzone
				if (!acceptable) {
					dt.dropEffect = 'none'
					dt.effectAllowed = 'null'
				}
			})
			this.text.DD.dropzones.off('drop').on('drop', function (event) {
				let e = event.originalEvent
				
				if (!that.text.DD.dropLoad) {
					return false
				}
					
				let range = null
				if (document.caretRangeFromPoint) {
					range = document.caretRangeFromPoint(e.clientX, e.clientY)
				} else if (e.rangeParent) {
					range = document.createRange()
					range.setStart(e.rangeParent, e.rangeOffset)
				}
				let sel = window.getSelection()
				sel.removeAllRanges()
				sel.addRange(range)

				$(sel.anchorNode).closest('.textTokenDropzone').get(0).focus()
				document.execCommand('insertHTML', false, '<param name="dragonDropMarker" />' + that.text.DD.dropLoad)
				sel.removeAllRanges()

				let DDM = $('param[name="dragonDropMarker"]')
				let insertSuccess = DDM.length > 0
				if (insertSuccess) {
					DDM.remove()
				}
				
				that.text.DD.dropLoad = null
				that.textBindDraggables()
				e.preventDefault()
			})
		},
		textValue () {
			let textValue = $(this.$refs.textInput).get(0).childNodes
			let result = ''
			textValue.forEach(function (item) {
				if (item.data !== undefined) {
					result += $.trim(item.data)
				} else {
					let chipText = $(item).children().text()
					result += ' %%' + chipText + '%% '
				}
			})
			return result
		},
		saveText () {
			let text = this.textValue()

			let query = '?value=' + encodeURI(text)
				+ '&language' + this.text.languageValue 
				+ '&speaker=' + this.text.speakerValue
				+ '&emotion=' + this.text.emotionValue 
				+ '&speed=' + this.text.speedValue / 100
				+ '&sourceId=' + this.sourceId
				+ '&title=' + this.text.save.name

			axios.post('http://localhost:50481/api/texts' + query)
				.then(response => {
					let id = response.data

					this.text.source = '2'
					this.getTexts()
					this.textSelected = id
				})
				.catch(error => {
					switch (error.response.status) {
					case 409:
						//title validation failed
						// this.upload.nameErrorState = true
						// this.upload.nameError.push(this.upload.nameErrorText)
						break
					}
				})
		},
		getTexts () {
			axios.get('http://localhost:50481/api/texts?sourceId=' + this.sourceId)
				.then(response => {
					this.text.items = response.data
				})
				.catch(error => {
					console.log(error.response)
				})
		},
		getTextHeaders () {
			return axios.get('http://localhost:50481/api/sources/' + this.sourceId + '/headers')
				.then(response => {
					this.text.headers = response.data
				})
				.catch(error => {
					console.log(error.response)
				})
		},
		initPreview () {
			window.AudioContext = window.AudioContext || window.webkitAudioContext
			this.preview.audioContext = new AudioContext()
			this.preview.headers = this.text.headers.map(header => {
				return {
					sortable: false, 
					text: header, 
					align: 'left'
				}
			})
			this.getPreviewItem(this.preview.pagination - 1)
		},
		getPreviewItem (offset) {
			this.preview.tableLoading = true
			let query = 'rows=1&offset=' + offset
			axios.get('http://localhost:50481/api/sources/' + this.sourceId + '/items?' + query).then(response => {
				let data = response.data
				this.preview.sourceItems = data.total
				this.preview.items = data.items
				
				this.preview.tableLoading = false
			}).catch(error => {
				console.log(error)
			})
		},
		getPreview () {
			this.preview.isLoading = true

			let url = 'http://localhost:50481/api/preview'
			let query = ''

			if (this.text.source === '1') {
				let text = this.textValue()
				query = '/text?text=' + encodeURI(text)
				+ '&language' + this.text.languageValue 
				+ '&speaker=' + this.text.speakerValue
				+ '&emotion=' + this.text.emotionValue 
				+ '&speed=' + this.text.speedValue / 100
				+ '&sourceId=' + this.sourceId
				+ '&offset=' + this.previewOffset
			} else if (this.text.source === '2') {
				query = '/saved-text?textId=' + this.textSelected
				+ '&sourceId=' + this.sourceId
				+ '&offset=' + this.previewOffset
			}

			let finalUrl = url + query

			axios({
				method: 'get',
				url: finalUrl,
				responseType: 'arraybuffer'
			}).then(response => {
				this.preview.isLoading = false
				this.playPreview(response.data)
			}).catch(error => {
				console.log(error)
			})
		},
		playPreview (arrayBuffer) {
			let that = this
			this.preview.audioContext.decodeAudioData(arrayBuffer, function (buffer) {
				let source = that.preview.audioContext.createBufferSource()
				source.buffer = buffer
				source.connect(that.preview.audioContext.destination)
				source.start(0)
			})
		},
		textUpload () {

		},
		backgroundUpload () {

		},
		saveCombination () {
			if (this.combinationSaveEnabled) {
				//check source type to indicate is it or save or update
				//if update - check for seleted combination 
			}
		},
		textFileChanged (files) {
			this.text.upload.correctFileFormat = true
			this.text.upload.file = _.first(files)
		},
		backgroundFileChanged (files) {
			this.background.upload.correctFileFormat = true
			this.background.upload.file = _.first(files)
		},
		uploadFile () {
			this.upload.active = true

			return new Promise((resolve, reject) => {
				let formData = new FormData()
				formData.append('name', this.upload.name)
				formData.append('base', this.upload.file)

				axios.post('http://localhost:50481/api/upload/base', formData)
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
							break
						}
						this.upload.active = false
					})
			}).then(uploadedBase => {
				this.flushFileUpload()

				this.bases.push(uploadedBase)
				this.baseSelected = uploadedBase.id
			})
		},
		flushFileUpload () {
			this.upload.correctFileFormat = true
			this.upload.active = false
			this.upload.name = ''
			this.upload.nameErrorState = false
			this.upload.nameError = []
			this.upload.file = null
		},
	}
}
</script>

<style scoped lang="scss">
.dropbox {
	border: 2px dashed grey;
	border-radius: 10px;
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

.text {
	line-height: 40px;
	border-radius: 10px;
}

.textInput {
	border: 2px solid grey;
	border-radius: 10px;
	height: 100%;
	padding: 10px 15px;
}

.textInput[contenteditable="true"] .textToken {
	-moz-user-select: none;
	-webkit-user-select: none;
	user-select: none; /* without this line, element can be dragged within itself! No-no! */
	-moz-user-drag: element;
	-webkit-user-drag: element;
	cursor: move !important; 
} 

.textToken {
	height: 26px;
	-webkit-touch-callout: none;
	-webkit-user-select: none;
	-khtml-user-select: none;
	-moz-user-select: none;
	-ms-user-select: none;
	user-select: none;
}
</style>