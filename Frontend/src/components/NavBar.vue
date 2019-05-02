<template>
  <v-toolbar app dark>
      <v-toolbar-title class = "NavigationToolBar">
        <span class="font-weight-light">GreetNGroup</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <div>
          <v-btn to="/Search">Search</v-btn>
          <v-btn to="/HelloWorld">HelloPage</v-btn>
          <v-btn to="/analysisdashboard">User Analysis</v-btn>
          <v-btn to="/findeventsforme">FindEventsForMe</v-btn>
          <v-btn v-if="isLoggedIn.isLogin" to="/CreateEvent">Create Event</v-btn>
      </div>
      <div>
      <v-menu v-if="isLoggedIn.isLogin" offset-y
              content-class="dropdown-menu"
              transition="slide-y-transition">
        <v-btn slot="activator" fab dark color="teal">
          <v-avatar dark>
            <span class="white--text headline">{{isLoggedIn.email[0]}}</span>
          </v-avatar>
        </v-btn>
        <v-list dense>
          <v-list-tile v-for="item in this.UserMenuItems"
                        :key="item.title"
                        route :to="item.route"
                        >
            <v-list-tile-title>{{item.title}}</v-list-tile-title>
          </v-list-tile>
        </v-list>
      </v-menu>
      </div>
  </v-toolbar>
</template>

<script>
import { store } from '@/router/request'
export default {
  name: 'NavBar',
  data () {
    return {
      isLoggedIn: store.state,
      UserMenuItems: [
        { title: 'Update Profile', route: '/updateprofile' },
        { title: 'Logout', route: '/logout' } 
      ]
    }
  },
  mounted () {
    store.isUserLogin()
    if (store.state.isLogin === true) {
      store.getEmail()
    }
  }
}
</script>
