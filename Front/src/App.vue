<template>
	<v-app light>
		<v-navigation-drawer clipped fixed app>
			<v-list dense>
				<v-list-tile>
					<v-list-tile-action>
						<v-icon>dashboard</v-icon>
					</v-list-tile-action>
					<v-list-tile-content>
						<v-list-tile-title>
							<router-link :to="{ name: 'dashboard' }" class="router-link">
								Инфо панель
							</router-link>
						</v-list-tile-title>
					</v-list-tile-content>
				</v-list-tile>
				<v-list-tile>
					<v-list-tile-action>
						<v-icon>group_add</v-icon>
					</v-list-tile-action>
					<v-list-tile-content>
						<v-list-tile-title>
							<router-link :to="{ name: 'events' }" class="router-link">
								Новая автокампания
							</router-link>
						</v-list-tile-title>
					</v-list-tile-content>
				</v-list-tile>
				<!-- <v-list-tile>
					<v-list-tile-action>
						<v-icon>settings</v-icon>
					</v-list-tile-action>
					<v-list-tile-content>
						<v-list-tile-title>Базы пользователей</v-list-tile-title>
					</v-list-tile-content>
				</v-list-tile>
				<v-list-tile>
					<v-list-tile-action>
						<v-icon>settings</v-icon>
					</v-list-tile-action>
					<v-list-tile-content>
						<v-list-tile-title>История</v-list-tile-title>
					</v-list-tile-content>
				</v-list-tile>
				<v-list-tile>
					<v-list-tile-action>
						<v-icon>settings</v-icon>
					</v-list-tile-action>
					<v-list-tile-content>
						<v-list-tile-title>Биллинг</v-list-tile-title>
					</v-list-tile-content>
				</v-list-tile> -->
			</v-list>
		</v-navigation-drawer>
		<v-toolbar app fixed clipped-left color="primary" class="theme--dark">
			<v-toolbar-title>Notificall</v-toolbar-title>
		</v-toolbar>
		<v-content>
			<v-container fluid fill-height>
				<v-layout flex>
					<v-flex xs12 offset-md1 md10>
						<transition name="slide">
							<router-view/>
						</transition>
					</v-flex>
				</v-layout>
			</v-container>
		</v-content>
		<v-footer app fixed>
			<span>&copy; 2018</span>
		</v-footer>
	</v-app>
</template>

<script>
export default {
	name: 'Notificall'
}
</script>

<style>
h1, h2 {
	font-weight: normal;
}
#app {
	font-family: 'Avenir', Helvetica, Arial, sans-serif;
	-webkit-font-smoothing: antialiased;
	-moz-osx-font-smoothing: grayscale;
	text-align: center;
	color: #2c3e50;
}
.router-link {
	text-decoration: none;
}
.container {
	max-width: 100%;
}
</style>
