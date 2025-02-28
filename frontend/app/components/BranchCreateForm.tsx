"use client"
import React, { useState, useEffect } from 'react'
import { GeocoderAutocomplete } from '@geoapify/geocoder-autocomplete'
import "@geoapify/geocoder-autocomplete/styles/round-borders-dark.css"

interface Props{
  index: number,
  getData: Function,
}

const BranchCreateForm = (props: Props) => {
  const [form, setForm] = useState({
    index: props.index,
    name: "",
    description: null,
    address: {
      countryName: "",
      stateName: "",
      cityName: "",
      streetName: null,
      houseNumber: "",
      apartmentNumber: null,
      postCode: "",
      lon: 0,
      lat: 0,
    },
  })

  var exists = false
  useEffect(() => {
    if (!exists) {
      let elem = document.getElementById(`autocomplete-${props.index}`)
      let api = process.env.GEOAPIFY_API;
      if (elem && api) {
        const autocomplete = new GeocoderAutocomplete(
          elem,
          api,
          {
            lang: 'en',
            filter: {
              'countrycode': ['pl'],
            },
          },
        );

        autocomplete.on("select", (location) => {
          setForm({
            ...form, address: {
              ...form.address,
              countryName: location.properties.country,
              stateName: location.properties.state,
              cityName: location.properties.city,
              streetName: location.properties.street ? location.properties.street : null,
              houseNumber: location.properties.housenumber,
              postCode: location.properties.postcode,
              lon: location.properties.lon,
              lat: location.properties.lat,
            }
          })
          props.getData(form)
        })
      }
      exists = true;
    }
  }, [])

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value })
    props.getData(form)
  }
  const handleAddressChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, address: { ...form.address, [e.target.name]: e.target.value } })
    props.getData(form)
  }

  return (
    <div className='flex flex-col gap-4'>
      <label htmlFor='name'>Branch name:</label>
      <input type='text' name='name' placeholder='Branch name' onChange={handleChange} required />
      <label htmlFor='description'>Description:</label>
      <input type='text' name='description' placeholder='Description' onChange={handleChange} />
      <label htmlFor='address'>Address:</label>
      <div id={`autocomplete-${props.index}`} className='autocomplete-container' style={{ position: 'relative' }}></div>
      <label htmlFor='apartmentNumber'>Apartment number:</label>
      <input type='text' name='apartmentNumber' placeholder='123' onChange={handleAddressChange} />
    </div>
  )
}

export default BranchCreateForm