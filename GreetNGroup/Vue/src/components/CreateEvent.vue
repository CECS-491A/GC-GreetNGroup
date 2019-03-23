<template>
    <div class="create-event">
        <v-app id="event-creation-form">
            <v-layout justify-center>
                <v-flex xs12 sm10 md8 lg6>
                    <v-card ref="form">
                        <v-card-text>
                            <v-text-field
                            ref="name"
                            v-model="name"
                            :rules="[() => !!name || 'This field is required',
                                    () => !!name && name.length <= 50 || 'Title must be less than 50 characters', titleCheck
                                    ]"
                            :error-messages="errorMessages"
                            label="Event Title"
                            placeholder="Pizza Party"
                            counter="50"
                            required
                            ></v-text-field>
                            <v-radio-group v-model="row" row>
                            <v-radio label="Online Event" value="isOnline"></v-radio>
                            <v-radio label="Physical Event" value="isPhysical"></v-radio>
                            </v-radio-group>
                            <v-text-field
                            ref="address"
                            v-model="address"
                            :rules="[
                                () => !!address || 'This field is required'
                            ]"
                            label="Address Line"
                            placeholder="Snowy Rock Pl"
                            required
                            ></v-text-field>
                            <v-text-field
                            ref="city"
                            v-model="city"
                            :rules="[() => !!city || 'This field is required', addressCheck]"
                            label="City"
                            placeholder="El Paso"
                            required
                            ></v-text-field>
                            <v-text-field
                            ref="state"
                            v-model="state"
                            :rules="[() => !!state || 'This field is required']"
                            label="State"
                            required
                            placeholder="Texas"
                            ></v-text-field>
                            <v-text-field
                            ref="zip"
                            v-model="zip"
                            :rules="[() => !!zip || 'This field is required']"
                            label="ZIP / Postal Code"
                            required
                            placeholder="79938"
                            ></v-text-field>
                            <v-menu
                            ref="date-menu"
                            v-model="menu"
                            :close-on-content-click="false"
                            :nudge-right="40"
                            :return-value.sync="date"
                            lazy
                            transition="scale-transition"
                            offset-y
                            full-width
                            min-width="290px"
                            >
                            <template v-slot:activator="{ on }">
                            <v-text-field
                                v-model="date"
                                ref="date"
                                :rules="[() => !!date || 'This field is required']"
                                label="Date of Event"
                                prepend-icon="event"
                                readonly
                                v-validate="'required'"
                                v-on="on"
                            ></v-text-field>
                            </template>
                            <v-date-picker v-model="date" no-title scrollable>
                            <v-spacer></v-spacer>
                            <v-btn flat color="primary" @click="menu = false">Cancel</v-btn>
                            <v-btn flat color="primary" @click="$refs.menu.save(date)">OK</v-btn>
                        </v-date-picker>
                        </v-menu>
                        </v-card-text>
                        <v-divider class="mt-5"></v-divider>
                        <v-card-actions>
                            <v-btn flat>Cancel</v-btn>
                            <v-spacer></v-spacer>
                            <v-slide-x-reverse-transition>
                            <v-tooltip
                                v-if="formHasErrors"
                                left
                            >
                                <template v-slot:activator="{ on }">
                                <v-btn
                                    icon
                                    class="my-0"
                                    @click="resetForm"
                                    v-on="on"
                                >
                                    <v-icon>refresh</v-icon>
                                </v-btn>
                                </template>
                                <span>Refresh form</span>
                            </v-tooltip>
                            </v-slide-x-reverse-transition>
                            <v-btn color="primary" flat @click="submit">Submit</v-btn>
                        </v-card-actions>
                    </v-card>
                </v-flex>
            </v-layout>
        </v-app>
    </div>
</template>

<script>
    new Vue({
        el: '#app',
        data: () => ({
            errorMessages: '',
            name: null,
            address: null,
            city: null,
            state: null,
            zip: null,
            country: null,
            formHasErrors: false
        }),

        computed: {
            form () {
            return {
                name: this.name,
                address: this.address,
                city: this.city,
                state: this.state,
                zip: this.zip,
                country: this.country
            }
            }
        },

        watch: {
            name () {
            this.errorMessages = ''
            }
        },

        methods: {
            addressCheck () {
            this.errorMessages = this.address && !this.name
                ? 'Hey! I\'m required'
                : ''

            return true
            },
            resetForm () {
            this.errorMessages = []
            this.formHasErrors = false

            Object.keys(this.form).forEach(f => {
                this.$refs[f].reset()
            })
            },
            submit () {
            this.formHasErrors = false

            Object.keys(this.form).forEach(f => {
                if (!this.form[f]) this.formHasErrors = true

                this.$refs[f].validate(true)
            })
            }
        }
    })
</script>