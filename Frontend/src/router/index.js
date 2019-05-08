/* eslint-disable */
import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import Logout from '@/components/Logout'
import Profile from '@/components/Profile'
import Welcome from '@/components/Welcome'
import EventPage from '@/components/EventPage'
import SearchPage from '@/components/Search'
import HelloWorld from '@/components/HelloWorld'
import CreateEvent from '@/components/CreateEvent'
import PageNotFound from '@/components/PageNotFound'
import UpdateProfile from '@/components/UpdateProfile'
import FindEventsForMe from '@/components/FindEventsForMe'
import ActivateProfile from '@/components/ActivateProfile'
import TermsConditions from '@/components/TermsConditions'
import AnalysisDashboard from '@/components/AnalysisDashboard'
import Axios from 'axios'

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
      name: 'welcome',
      component: Welcome
    },
    {
      path: '*',
      component: PageNotFound
    },
    {
      path: '/search',
      name: 'search',
      component: SearchPage
    },
    {
      path: '/findeventsforme',
      name: 'findeventsforme',
      component: FindEventsForMe
    },
    {
      path: '/createevent',
      name: 'createevent',
      component: CreateEvent,
      meta: {
        isLoggedIn: true,
        canCreateEvents: true
      }
    },
    {
      path: '/helloworld',
      name: 'helloworld',
      component: HelloWorld
    },
    {
      path: '/analysisdashboard',
      name: 'useranalysisdashboard',
      component: AnalysisDashboard,
      meta: {
        isLoggedIn: true,
        isAdmin: true
      }
    },
    {
      path: '/profile/:id',
      name: 'profile',
      component: Profile
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    },
    {
      path: '/updateprofile',
      name: 'updateprofile',
      component: UpdateProfile
    },
    {
      path: '/activateprofile',
      name: 'activateprofile',
      component: ActivateProfile
    },
    {
      path: '/logout',
      name: 'logout',
      component: Logout,
      meta:{
        isLoggedIn: true
      }
    },
    {
      path: '/eventpage/:name',
      name: 'eventpage',
      component: EventPage
    },
    {
      path: '/termsandconditions',
      name: 'termsconditions',
      component: TermsConditions
    }
  ]
})

/* Before a user reaches certain paths, their JWT must first be checked to ensure they have the proper claims
 * to view the content
 */
router.beforeEach((to, from, next) => {
  if (to.matched.some(value => value.meta.isLoggedIn)) {
    if (localStorage.getItem('token') == null){
      alert('Please log in to view this page!')
      next('/')
    }
    else{
      if (to.method.some(value => value.meta.isAdmin)) {
        let message = ''
        let canView = async() => {
          Axios.post('http://localhost:62008/api/jwt/check', {
            jwt: localStorage.getItem('token'),
            claimsToCheck: ['isAdmin']
          }).then((response) => {
            canView = response.status,
            message = response
          })
        }
        if(canView == 200) {
          alert(message)
          next()
        }
        else{
          alert(message)
          next('/')
        }
      }
      if(to.method.some(value => value.meta.canCreateEvents)) {
        let message = ''
        let canMakeEvents = async() => {
          Axios.post('http://http://localhost:62008/api/JWT/check', {
            jwt: localStorage.getItem('token'),
            claimsToCheck: ['canCreateEvents']
          }).then((response) => {
            canMakeEvents = response.status,
            message = response
          })
        }
        if(canMakeEvents == 200) {
          alert(message)
          next()
        }
        else{
          alert(message)
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

// This will make sure that only successful entries to a certain path will be logged
router.afterEach((to, from) => {
  let ipAddress = localStorage.getItem('ip')
  Axios.post('http://localhost:62008/api/logclicks', {
    Jwt: localStorage.getItem('token'),
    Ip: ipAddress,
    StartPoint: 'https://www.greetngroup.com' + from.fullPath.toString(),
    EndPoint: 'https://www.greetngroup.com' + to.fullPath.toString()
  })
})

export default router
