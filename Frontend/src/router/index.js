/* eslint-disable */
import Vue from 'vue'
import Router from 'vue-router'
import SearchPage from '@/components/Search'
import HelloWorld from '@/components/HelloWorld'
import PageNotFound from '@/components/PageNotFound'
import TestLogin from '@/components/TestLogin'
import AnalysisDashboard from '@/components/AnalysisDashboard'
import CreateEvent from '@/components/CreateEvent'
import Profile from '@/components/Profile'
import UpdateProfile from '@/components/UpdateProfile'
import Home from '@/components/Home'
import Welcome from '@/components/Welcome'
// import Axios from 'axios'

Vue.use(Router)

/*
  The path '*' is to create a catch all(default) for
  route paths that are unknown (not specified),
  to lead to our 404 page
*/
const router = new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'Welcome',
      component: Welcome
    },
    {
      path: '/Search',
      name: 'Search',
      component: SearchPage
    },
    {
      path: '/CreateEvent',
      name: 'CreateEvent',
      component: CreateEvent
    },
    {
      path: '/testlogin',
      name: 'TestLogin',
      component: TestLogin
    },
    {
      path: '/HelloWorld',
      name: 'HelloWorld',
      component: HelloWorld
    },
    {
      path: '*',
      component: PageNotFound
    },
    {
      path: '/analysisdashboard',
      name: 'UserAnalysisDashboard',
      component: AnalysisDashboard,
      meta: {
        isLoggedIn: true,
        isAdmin: true
      }
    },
    {
      path: '/profile/:id',
      name: 'Profile',
      component: Profile
    },
    {
      path: '/Home/:token',
      name: 'Home',
      component: Home
    },
    {
      path: '/updateprofile',
      name: 'UpdateProfile',
      component: UpdateProfile
    }
  ]
})
/*
router.beforeEach((to, from, next) => {
  if (to.matched.some(value => value.meta.isLoggedIn)) {
    if (localStorage.getItem('JWT') == null){
      next('/testlogin')
    }
    else{
      if (to.method.some(value => value.meta.isAdmin)) {
        var canView = async() => {
          Axios.post('http://localhost:50884/api/JWT/check', {
            jwt: localStorage.getItem('JWT'),
            claimsToCheck: ['isAdmin']
          })
        }
        if(canView == true) {
          next()
        }
        else{
          next('/')
        }
      }
      else{
        next()
      }
    }
  }
  else{
    next()
  }
})
*/
export default router
